using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Microsoft.AspNetCore.SignalR.Client;
using RTTicTacToe.Forms.Messages;
using RTTicTacToe.WebApi.Models.Hubs;
using Xamarin.Forms;

namespace RTTicTacToe.Forms.Services
{
    public class GameHubService : IGameHubService, IGameHubEndpoint
    {
        #region Fields

        HubConnection _hubConnection;
        private readonly IMessagingCenter _messagingCenter;

        #endregion

        #region Properties

        public bool IsOnline => _hubConnection?.State == HubConnectionState.Connected;
        public bool IsReconnecting { get; protected set; }

        #endregion

        #region Constructor

        public GameHubService(IMessagingCenter messagingCenter)
        {
            _messagingCenter = messagingCenter;

            ConfigureHub();
        }

        #endregion

        #region Methods

        private void ConfigureHub()
        {
            _hubConnection = new HubConnectionBuilder()
                                .WithUrl($"{App.AzureBackendUrl}/api/gamehub")
                                .Build();

            _hubConnection.KeepAliveInterval = new TimeSpan(0, 0, 5);

            _hubConnection.Closed += async (error) =>
            {
                try
                {
                    SetIsReconnecting(true);

                    await UserDialogs.Instance.AlertAsync("Hub Connection closed. Will try to reconnect...", "Hub Connection", "Ok");

                    //ry to reconnect
                    await Task.Delay(new Random().Next(0, 5) * 1000);
                    await StartHubAsync();
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync($"trying to reconnect - connection.StartAsync(): {ex.Message}", "Hub Connection", "Ok");
                }
                finally
                {
                    SetIsReconnecting(false);
                }
            };

            _hubConnection.On(nameof(IGameHub.GameCreated), () =>
            {
                _messagingCenter.Send(this, nameof(GameCreatedMessage), new GameCreatedMessage());
            });

            _hubConnection.On<Guid>(nameof(IGameHub.PlayerJoinedGame), (gameId) =>
            {
                _messagingCenter.Send(this, nameof(PlayerJoinedGameMessage), new PlayerJoinedGameMessage(gameId));
            });

            _hubConnection.On<Guid>(nameof(IGameHub.MovementMade), (gameId) =>
            {
                _messagingCenter.Send(this, nameof(MovementMadeMessage), new MovementMadeMessage(gameId));
            });

            _hubConnection.On<Guid>(nameof(IGameHub.GameFinished), (gameId) =>
            {
                _messagingCenter.Send(this, nameof(GameFinishedMessage), new GameFinishedMessage(gameId));
            });
        }

        public async Task StartHubAsync()
        {
            try
            {
                if (_hubConnection.State == HubConnectionState.Disconnected)
                {
                    SetIsReconnecting(true);

                    await _hubConnection.StartAsync();
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync($"Hub already connected", "Hub Connection", "Ok");
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync($"connection.StartAsync(): {ex.Message}", "Hub Connection", "Ok");
            }
            finally
            {
                SetIsReconnecting(false);
            }
        }

        public async Task RegisterForGame(Guid gameId)
        {
            await _hubConnection.InvokeAsync(nameof(IGameHubEndpoint.RegisterForGame), gameId);
        }

        public async Task UnregisterForGame(Guid gameId)
        {
            await _hubConnection.InvokeAsync(nameof(IGameHubEndpoint.RegisterForGame), gameId);
        }

        private void SetIsReconnecting(bool value)
        {
            IsReconnecting = value;
            _messagingCenter.Send(this, nameof(ConnectionStateChangedMessage), new ConnectionStateChangedMessage());
        }

        #endregion
    }
}
