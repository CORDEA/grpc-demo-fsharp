namespace GrpcDemoClient

open Grpc.Core
open Grpcdemo
open System
open Google.Protobuf

module GrpcDemoImpl =
    type GrpcDemoImpl(client: GrpcDemo.GrpcDemoClient) =
        
        member this.Demo() =
            let nonce = GrpcDemoUtils.getNonce()
            Console.WriteLine("request: " + nonce)
            let obj = new Grpcdemo.Object(StringDemo = "test")
            let request = new Request(Nonce = nonce, FloatDemo = 0.0f)
            request.MapDemo.Add("test", obj)
            client.Demo(request) |> fun x -> String.Format("response: {0}", x.Nonce) |> Console.WriteLine

        member this.RepeatedDemo() =
            let nonce = GrpcDemoUtils.getNonce()
            Console.WriteLine("request: " + nonce)
            new RepeatedRequest(Nonce = nonce)
                |> fun x ->
                    for i = 0 to 10 do
                        x.IntDemo.Add(i)
                        x.ObjectDemo.Add(new Grpcdemo.Object(StringDemo = "test" + i.ToString()))
                    x
                |> client.RepeatedDemo
                |> fun x -> String.Format("response: {0}", x.Nonce)
                |> Console.WriteLine

        member this.StreamDemo() =
            let ay = client.StreamDemo()
            for i = 0 to 10 do
                new StreamRequest(Nonce = GrpcDemoUtils.getNonce(), FloatDemo = 0.0f)
                        |> fun x ->
                            String.Format("request{0}: {1}", i, x.Nonce)
                            |> Console.WriteLine
                            x
                        |> ay.RequestStream.WriteAsync
                        |> Async.AwaitTask
                        |> Async.RunSynchronously
            ay.RequestStream.CompleteAsync().Wait()

            let stream = ay.ResponseStream
            let mutable i = 0
            while stream.MoveNext(Threading.CancellationToken.None) |> Async.AwaitTask |> Async.RunSynchronously do
                stream.Current
                    |> fun x -> String.Format("response{0}: {1}", i, x.Nonce)
                    |> Console.WriteLine
                i <- i + 1