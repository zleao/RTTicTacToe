using System;
using CQRSlite.Domain;
using RTTicTacToe.CQRS.ReadModel.Events;
using RTTicTacToe.CQRS.Utilities;
using RTTicTacToe.CQRS.WriteModel.Domain.Exceptions;

namespace RTTicTacToe.CQRS.WriteModel.Domain
{
    public class GameAggregate : AggregateRoot
    {
        #region Fields

        private bool _gameStarted;
        private bool _gameFinished;
        private readonly int[][] _gameBoard = new int[][] { new int[3] , new int[3] };

        private Guid _player1Id = Guid.Empty;
        private Guid _player2Id = Guid.Empty;
        private int _playersTurn;

        #endregion

        #region Constructor

        private GameAggregate(){}
        public GameAggregate(Guid id, string name)
        {
            if (id == Guid.Empty)
            {
                throw new GameIdInvalidException();
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new GameNameInvalidException();
            }

            Id = id;
            ApplyChange(new GameCreated(id, name));
        }

        #endregion

        #region Commands Handling

        public void MakeMovement(Guid playerId, int movementX, int movementY)
        {
            if (!_gameStarted)
            {
                throw new GameNotStartedException();
            }

            if (_gameFinished)
            {
                throw new GameAlreadyFinishedException();
            }

            if (_player1Id != playerId && _player2Id != playerId)
            {
                throw new MovementInvalidPlayerIdException();
            }

            if (movementX < 0 || movementX > 2 || movementY < 0 || movementY > 2)
            {
                throw new MovementOutOfBoundsException();
            }

            if (_gameBoard[movementX][movementY] != 0)
            {
                throw new MovementAlreadyTakenException();
            }

            if (_playersTurn == 1) //Handle a play for player 1
            {
                if (_player1Id != playerId)
                {
                    throw new MovementWrongPlayerException();
                }
            }
            else if (_playersTurn == 2) //Handle a play for player 2
            {
                if (_player2Id != playerId)
                {
                    throw new MovementWrongPlayerException();
                }
            }
            
            if (_playersTurn == 1) //Handle a play for player 1
            {
                if (_player1Id != playerId)
                {
                    throw new MovementWrongPlayerException();
                }
            }
            else if (_playersTurn == 2) //Handle a play for player 2
            {
                if (_player2Id != playerId)
                {
                    throw new MovementWrongPlayerException();
                }
            }

            ApplyChange(new MovementMade(Id, playerId, _playersTurn, movementX, movementY));
        }

        public void RegisterPlayer(Guid playerId, string playerName)
        {
            if (_gameFinished)
            {
                throw new GameAlreadyFinishedException();
            }

            if (_gameStarted)
            {
                throw new GameAlreadyStartedException();
            }

            if (playerId == Guid.Empty)
            {
                throw new PlayerIdInvalidException();
            }

            if (_player1Id == playerId || _player2Id == playerId)
            {
                throw new PlayerAlreadyRegisteredInTheGameException();
            }

            var playerNumber = (_player1Id == Guid.Empty) ? 1 : 2;

            ApplyChange(new PlayerRegistered(Id, playerId, playerName, playerNumber));
        }

        #endregion

        #region Events Handling

        private void Apply(GameCreated e)
        {
            Id = e.Id;
        }

        private void Apply(PlayerRegistered e)
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

        private void Apply(MovementMade e)
        {
            _gameBoard[e.X][e.Y] = _playersTurn;

            if (GameHelper.CheckGameFinished(_gameBoard))
            {
                _gameFinished = true;
            }
            else
            {
                _playersTurn = (_playersTurn == 1) ? 2 : 1;
            }
        }

        #endregion
    }
}
