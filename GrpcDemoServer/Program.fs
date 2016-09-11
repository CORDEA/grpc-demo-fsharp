namespace GrpcDemoClient

open Grpc.Core
open Grpcdemo
open System

module Program = 
    [<EntryPoint>]
    let main argv = 
        let port = 50051
        let server = new Server()
        server.Services.Add(GrpcDemo.BindService(new GrpcDemoImpl.GrpcDemoImpl()))
        server.Ports.Add(new ServerPort("localhost", port, ServerCredentials.Insecure)) |> ignore
        server.Start()

        Console.WriteLine("Press any key to stop the server...")
        Console.ReadKey() |> ignore

        server.ShutdownAsync().Wait()
        0