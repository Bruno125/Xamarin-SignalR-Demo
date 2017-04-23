# SignalR with Xamarin

## Introducción

Realizar una prueba de concepto en la que se pueda demostrar el uso de SignalR entre dispositivos Android y iOS,
mediante la creación de un chat de dispositivos cercanos

## Requerimientos

### Para el desarrollo
- Xamarin Studio Community 6.3
- XCode 8.2

### Para las pruebas
- 1 Dispositivo Android 2.3
- 1 Dispositivo iOS 8.0
- Conexión a internet en ambos dispositivos

## Definición de conceptos

- __Xamarin:__ plataforma que nos permite desarrollar aplicaciones nativas para Android, iOS, y Windows Phone, con la ventaja
de poder desarrollar para todas esas plataformas en un solo proyecto, con una única fuente de código (escrito
en C#)
  - _Xamarin Forms_: enfoque que te permite desarrollar las interfaces visuales usando un único código fuente. Xamarin se encarga
  de generar las interfaces correspondientes para cada plataforma automáticamente.
  - _Xamarin.iOS & Xamarin.Android_: enfoque en el cual puedes reutilizar la lógica de negocio del proyecto, pero el desarrollo
  de la interfaz se hace por separado. Te da un poco de flexibilidad al momento de diseñar, pero implica más trabajo. Este es el
  enfoque que utilizamos en la aplicación.
- __SignalR__: _"es una librería para desarrolladores ASP.NET, que hace increíblemente simple añadir funcionalidad en tiempo real
a tus aplicaciones"_ (fuente: [signalr.net](http://signalr.net/))

## Funcionamiento

1. Cada dispositivo, al momento de abrir la aplicación, solicitará al [servidor](https://github.com/Bruno125/chat-signalR) abrir una conexión para poder recibir y enviar mensajes. En ese momento, pasan a ser "clientes" de la aplicación.
2. Uno de los dispositivos enviará un mensaje al servidor.
3. El servidor recibirá el mensaje y se lo reenviará a todos los clientes que tenga registrados en ese momento.
4. Las aplicaciones reciben los mensajes. En este caso, la demo mostrará el contenido y tamaño (en bytes) del mensaje, y el tiempo que tardó en realizar toda la comunicación.

## Diagramas

### Diagrama de secuencia

![alt tag](https://raw.githubusercontent.com/Bruno125/Xamarin-SignalR-Demo/master/Documentation/Diagrams/Sequence%20diagram.png)

### Diagrama de componentes

![alt tag](https://raw.githubusercontent.com/Bruno125/Xamarin-SignalR-Demo/master/Documentation/Diagrams/Components%20diagram.png)

### Diagrama de despliegue

![alt tag](https://raw.githubusercontent.com/Bruno125/Xamarin-SignalR-Demo/master/Documentation/Diagrams/Deployment%20diagram.png)

## Implementación

### Shared Project (Lógica de negocio)

En el proyecto compartido, crearemos una clase [SignalConnector](), la cual será el punto central por el cual las aplicaciones
Android / iOS podrán conectarse con el servidor web.

Al momento de crear una instancia de esta clase, estableceremos una conexión con el servidor (que también utiliza SignalR).
Para este caso, haremos referencia al Hub llamado `ChatHub`. Luego, nos suscribiremos al evento `addNewMessageToPage`, el
cual nos devolverá el mensaje enviado y el timestamp de cuando este fue enviado.

```C#
HubConnection chatConnection;
IHubProxy SignalRChatHub;

public SignalConnector(Action<Response> listener)
{
  chatConnection = new HubConnection("http://chatsignalrtst.apphb.com/",true);
  SignalRChatHub = chatConnection.CreateHubProxy("ChatHub");

  SignalRChatHub.On<string, string>("addNewMessageToPage", (timestamp,message) =>
  {
    var response = new Response() { Size = MeasureSize(message), Delay = MeasureDelay(timestamp), Message = message };
    listener(response);
  });

}

public async virtual void JoinChat()
{
  try
  {
    await chatConnection.Start();
  }
  catch (Exception e)
  {
    Console.WriteLine(e.Message);
  }
}
```

Cuando el usuario envíe un mensaje desde la interfaz de la aplicación, invocará el método `Send(string message)`. En ese 
momento, enviaremos el mensaje `Send` al hub que registramos previamente.

```C#
public async void Send(string message)
{
  if (chatConnection.State == ConnectionState.Connected)
  {
    var timestamp = CreateTimestamp();
    await SignalRChatHub.Invoke("Send", timestamp, message);
  }
}

private static readonly string DateFormat = "yyyy-MM-dd hh:mm:ss.ffffff";
private string CreateTimestamp()
{
  return DateTime.Now.ToString(DateFormat);

}
```

### Xamarin.Android

1. Para empezar, añadiremos SignalR al proyecto por medio de NuGet. Tenemos que dar click derecho en el proyecto > Agregar > Agregar paquete NuGet





2. Luego, abriremos la clase de `MainActivity.cs` y en el método `OnCreate` instanciaremos la clase `SignalConnector`

```C#
Connector = new SignalConnector((Response response) => RunOnUiThread(() =>
{
  //Actualiza la lista de mensajes
  adapter.Add(response);
}));

Connector.JoinChat();
```

3. Asimismo, cuando el usuario presione el botón "Enviar" invocaremos al método `Send` del conector

```C#
Button button = FindViewById<Button>(Resource.Id.ChatButton);
button.Click += delegate
{
  if (string.IsNullOrEmpty(input.Text))
    return;

  Connector.Send(input.Text);
  input.Text = "";
};
```

4. Implementamos la clase [`ResponseAdapter`](Droid/ResponseAdapter.cs), la cual únicamente se encargará de mostrar las respuestas en la vista.

### Xamarin.iOS

1. Añadimos el paquete de SignalR de la misma forma que en Android.

2. Luego, abriremos la clase de `ViewController.cs` y en el método `ViewDidLoad` instanciaremos la clase `SignalConnector`

```C#
Connector = new SignalConnector((Response response) => RunOnUiThread(() =>
{
  //Actualiza la lista de mensajes
  source.Add( response);
  TableView.ReloadData();
}));

Connector.JoinChat();
```

3. Asimismo, cuando el usuario presione el botón "Enviar" invocaremos al método `Send` del conector

```C#
SendButton.TouchDown += (sender, e) =>
{
  if (string.IsNullOrEmpty(MessageTextField.Text))
    return;

          Connector.Send(MessageTextField.Text);
  MessageTextField.Text = "";
  MessageTextField.ResignFirstResponder();
};
```

4. Implementamos las clases [`MessagesDataSource`](iOS/MessagesDataSource.cs) y [`ResponseEntryCell`](iOS/ResponseEntryCell.cs) , las cuales únicamente se encargarán de mostrar los mensajes en la vista.

## Bibliografía

- Telerik Developers. Página de empresa que ofrece soluciones de software y desarrolla guías de implementación de diferentes 
tecnologías. (http://developer.telerik.com/products/real-time-mobile-apps-with-appbuilder-xamarin-and-signalr/) Consulta: 6 de Abril del 2017

- Microsoft Documents. Página donde Microsoft elabora tutoriales y documenta la implementación de sus tecnologías. 
(https://docs.microsoft.com/en-us/aspnet/signalr/overview/getting-started/tutorial-getting-started-with-signalr-and-mvc) Consulta: 7 de Abri del 2017

- Xamarin Forums. Página donde se puede solicitar ayuda al equipo de Xamarin sobre problemas de implementación sobre Xamarin.
(https://forums.xamarin.com/discussion/3506/signalr-connection-issues) Consulta: 7 de Abril del 2017

- Xamarin Developers. Página donde se documenta todo el material necesario para que los desarrolladores puedan comenzar a trabajar
con Xamarin. (https://developer.xamarin.com/guides/cross-platform/application_fundamentals/shared_projects/) Consulta: 10 de Abril de 2017

