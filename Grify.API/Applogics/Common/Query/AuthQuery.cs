using Common.Identity;
using Common.Identity.Models;
using Common.Model;
using Common.Persistence;
using Dapper;
using Grify.API.Models;
using MediatR;
using System.Data;

namespace Grify.API.Applogics.Common.Query
{
    public class AuthQuery : IRequest<ResponseModel>
    {
        public string MobileNo { get; set; }
        protected class AuthQueryHandler : IRequestHandler<AuthQuery, ResponseModel>
        {
            private readonly IDal _dal;
            private readonly IIdentityService _identityService;
            public AuthQueryHandler(IDal dal, IIdentityService identityService)
            {
                _dal = dal;
                _identityService = identityService;
            }
            public async Task<ResponseModel> Handle(AuthQuery request, CancellationToken cancellationToken)
            {
                var token = _identityService.GenerateJwtToken(new LoginModel { MobileNo = request.MobileNo });
                return new ResponseModel
                {
                    Code = 1,
                    Message = "Success",
                    Data = new Token
                    {
                        token = token.Result
                    }
                };
            }
        }
    }

}

public class Token
{
    public string token { get; set; }
}
