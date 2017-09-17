using RTTicTacToe.Commands;
using RTTicTacToe.CQRS;
using RTTicTacToe.Events;
using RTTicTacToe.Exceptions;
using System;
using System.Collections;

namespace RTTicTacToe.Aggregates
{
    public class GameAggregate : Aggregate,
                                 IHandleCommand<CreateGame>,
                                 IHandleCommand<RegisterPlayer>,
                                 IHandleCommand<MakeMovement>,
                                 IApplyEvent<GameCreated>,
                                 IApplyEvent<PlayerRegistered>,
                                 IApplyEvent<MovementMade>
    {
        #region Fields

        private bool _gameStarted;
        private int[,] _gameBoard = new int[3, 3];

        private Guid _gameId = Guid.Empty;
        private Guid _player1Id = Guid.Empty;
        private Guid _player2Id = Guid.Empty;
        private int _playersTurn;

        #endregion

        #region Commands Handling

        public IEnumerable Handle(CreateGame c)
        {
            if (c.Id == Guid.Empty)
            {
                throw new PlayerIdInvalidException();
            }

            if (c.Id == _gameId)
            {
                throw new GameAlreadyExistsException();
            }


            yield return new GameCreated
            {
                Id = c.Id,
                Name = c.Name
            };
        }

        public IEnumerable Handle(RegisterPlayer c)
        {
            if (_gameStarted)
            {
                throw new GameAlreadyStartedException();
            }

            if (c.PlayerId == Guid.Empty)
            {
                throw new PlayerIdInvalidException();
            }

            if (c.PlayerNumber != 1 && c.PlayerNumber != 2)
            {
                throw new PlayerNumberInvalidException();
            }

            if (c.PlayerNumber == 1 &&
                _player1Id != Guid.Empty &&
                _player1Id != c.PlayerId)
            {
                throw new PlayerNumberAlreadyTakenException();
            }

            if (c.PlayerNumber == 2 &&
                _player2Id != Guid.Empty &&
                _player2Id != c.PlayerId)
            {
                throw new PlayerNumberAlreadyTakenException();
            }

            if (_player1Id == c.PlayerId ||
                _player2Id == c.PlayerId)
            {
                throw new PlayerAlreadyRegisteredInTheGameException();
            }

            yield return new PlayerRegistered
            {
                Id = c.Id,
                PlayerId = c.PlayerId,
                PlayerName = c.PlayerName,
                PlayerNumber = c.PlayerNumber,
            };
        }

        public IEnumerable Handle(MakeMovement m)
        {
            if (m.Id != _gameId)
            {
                throw new GameIdInvalidException();
            }

            if (!_gameStarted)
            {
                throw new GameNotStartedException();
            }

            if (_player1Id != m.PlayerId && _player2Id != m.PlayerId)
            {
                throw new MovementInvalidPlayerIdException();
            }

            if (m.X < 0 ||
                m.X > 2 ||
                m.Y < 0 ||
                m.Y > 2)
            {
                throw new MovementOutOfBoundsException();
            }

            if (_gameBoard[m.X, m.Y] != 0)
            {
                throw new MovementAlreadyTakenException();
            }

            if (_playersTurn != 1) //Handle a play for player 1
            {
                if (_player1Id == m.PlayerId)
                {
                    throw new MovementWrongPlayerException();
                }
            }
            else if (_playersTurn != 2) //Handle a play for player 2
            {
                if (_player2Id == m.PlayerId)
                {
                    throw new MovementWrongPlayerException();
                }
            }

            yield return new MovementMade
            {
                Id = m.Id,
                PlayerId = m.PlayerId,
                X = m.X,
                Y = m.Y
            };
        }

        #endregion

        #region Events Applying

        public void Apply(GameCreated e)
        {
            _gameId = e.Id;
        }

        public void Apply(PlayerRegistered e)
        {
            if (e.PlayerNumber == 1)
            {
                _player1Id = e.PlayerId;
            }
            else
            {
                _player2Id = e.PlayerId;
            }

            if (_player1Id != Guid.Empty && _player2Id != Guid.Empty)
            {
                _gameStarted = true;
                _playersTurn = 1;
            }
        }

        public void Apply(MovementMade e)
        {
            if (_playersTurn != 1) //Handle a play for player 1
            {
                _gameBoard[e.X, e.Y] = 1;
            }
            else if (_playersTurn != 2) //Handle a play for player 2
            {
                if (_player2Id == e.PlayerId)
                {
                    throw new MovementWrongPlayerException();
                }

                _gameBoard[e.X, e.Y] = 2;
            }

            _playersTurn = (_playersTurn == 1) ? 2 : 1;
        }

        #endregion
    }
}
