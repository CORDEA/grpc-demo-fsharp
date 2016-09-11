namespace GrpcDemoClient

open Grpc.Core
open Grpcdemo
open System
open Google.Protobuf
open System.Threading.Tasks

module GrpcDemoImpl =
    type GrpcDemoImpl () =
        inherit GrpcDemo.GrpcDemoBase()

        override this.Demo(request: Request, context : ServerCallContext) : Task<Response> =
            Console.WriteLine("----received----")
            new Response(Nonce = request.Nonce, BytesDemo = ByteString.CopyFromUtf8("test"), EnumDemo = Enum.Demo1) |> Task.FromResult

        override this.RepeatedDemo(request : RepeatedRequest, context : ServerCallContext) : Task<RepeatedResponse> =
            Console.WriteLine("----received----")
            new RepeatedResponse(Nonce = request.Nonce)
                |> fun x ->
                    for i = 0 to 10 do
                        x.StringDemo.Add(String.Format("test{0}", i))
                    x
                |> Task.FromResult

        override this.StreamDemo(requestStream : IAsyncStreamReader<StreamRequest>, responseStream : IServerStreamWriter<StreamResponse>, context : ServerCallContext) =
            Task.Run(
                fun _ ->
                    Console.WriteLine("----received----")
                    while requestStream.MoveNext(Threading.CancellationToken.None) |> Async.AwaitTask |> Async.RunSynchronously do
                        let request = requestStream.Current
                        StreamResponse(Nonce = request.Nonce, EnumDemo = Enum.Demo2)
                            |> responseStream.WriteAsync
                            |> Async.AwaitTask
                            |> Async.RunSynchronously
            )