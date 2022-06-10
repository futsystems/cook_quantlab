using Grpc.Core;
using System.Threading.Tasks;
using GrpcBase = Grpc.Health.V1;

namespace UniCryptoLab.Grpc.API
{
    public class HealthServiceImpl: GrpcBase.Health.HealthBase
    {
        public static GrpcBase.HealthCheckResponse.Types.ServingStatus Status = GrpcBase.HealthCheckResponse.Types.ServingStatus.Serving;

        public override Task<GrpcBase.HealthCheckResponse> Check(GrpcBase.HealthCheckRequest request, ServerCallContext context)
        {
            return Task.Run(() => {

                return new GrpcBase.HealthCheckResponse()
                {
                    Status = Status,
                };
            });
        }

        public override Task Watch(GrpcBase.HealthCheckRequest request, IServerStreamWriter<GrpcBase.HealthCheckResponse> responseStream, ServerCallContext context)
        {
            return Task.Run(() => {

                return new GrpcBase.HealthCheckResponse()
                {
                    Status = Status,
                };
            });
        }

        
    }
}
