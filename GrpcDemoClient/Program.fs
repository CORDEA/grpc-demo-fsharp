namespace GrpcDemoClient

open Grpc.Core
open Grpcdemo
open System

module Program = 
    [<EntryPoint>]
    let main argv = 
        let channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure)
        let client = new GrpcDemoImpl.GrpcDemoImpl(new GrpcDemo.GrpcDemoClient(channel))

        client.Demo()
        client.RepeatedDemo()
        client.StreamDemo()

        Console.WriteLine("Press any key to stop the client...")
        Console.ReadKey() |> ignore

        channel.ShutdownAsync().Wait()
        0
