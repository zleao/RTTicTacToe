# RTTicTacToe
Fullstack implementation of an online/realtime TicTacToe game. The purpose is to test some tools and frameworks. The solution provides a WebApi (currently hosted on [Azure](https://rttictactoe.azurewebsites.net/swagger)) and a frontend with a basic implementation of the TicTacToe game.

### **Always refer to the readme file of the [master](https://github.com/zleao/RTTicTacToe/tree/master) branch, to get the most up-to-date info.**

## Backend
Simple Asp.NetCore API with a CQRS/ES pattern. For real time communication, it uses SignalR. The data is persisted in a SQLite database with the use of EFCore (both for EventSourcing and read datamodel).

**Key technologies:**
- .Net 5
- CQRSLite v1.33.0
- Swashbuckle.AspNetCore v6.1.4
- Microsoft.AspNetCore.SignalR.Core (1.1.0)
- EFCore v5.0.8
- EFCore.Sqlite v5.0.8

## Frontend
Cross platform client.
- Xamarin Forms 5 with support for **iOS**, **Android** and **UWP**


## Run the code
Project developed with VS2019. Supported by VS2019+
There are 2 solutions, one for backend and the other for frontend.
You can run the api locally or use the one I'm currently hosting in Azure ([https://rttictactoe.azurewebsites.net/swagger](https://rttictactoe.azurewebsites.net/swagger)).

**Note:** At any given time this endpoint might be deleted without prior notice.

### Backend
The main api project is the `RTTicTacToe.WebApi`. You can run the api locally (it should work without any special configuration) or publish it to a hosting service.

### Frontend
Since the frontend is implemented with Xamarin Forms, you can choose one of the implemented platforms and run in a simulator or real device. The code should compile and run as-is. The only required change, is the base url for the api endpoint. To configure it, just go `RTTicTacToe.Forms/App.cs` and set the static string `AzureBackendUrl`. By default it's pointing to the Azure hosted web app.

## Future tryouts
### Backend
- Try [EventFlow](https://eventflow.readthedocs.io/) for the CQRS/ES handling
- Use a specialized framework to store the data from EventSourcing, (e.g. [NEventStore](https://github.com/NEventStore/NEventStore))
- Add logic to revert movements using CQRS features
### Frontend
- MAUI implementation