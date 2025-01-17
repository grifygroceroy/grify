using Common.Model;
using Common.Persistence;
using Dapper;
using Grify.API.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Grify.API.Applogics.Common.Query
{
    public class HomeQuery : IRequest<ResponseModel>
    {
        public string ItemName { get; set; } = string.Empty;
        protected class HomeQueryHandler : IRequestHandler<HomeQuery, ResponseModel>
        {
            private readonly IDal _dal;
            public HomeQueryHandler(IDal dal)
            {
                _dal = dal;
            }
            public async Task<ResponseModel> Handle(HomeQuery request, CancellationToken cancellationToken)
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Search", request.ItemName);
                var queryResult = await _dal.RunMultipleQueryAsync("[dbo].[GetItem]", commandType: CommandType.StoredProcedure, parameters: queryParameters).ConfigureAwait(false);
                var itemsModel = (List<itemsModel>)await queryResult.Item2.ReadAsync<itemsModel>() ?? new List<itemsModel>();
                var SubItemModel = (List<SubItemModel>)await queryResult.Item2.ReadAsync<SubItemModel>() ?? new List<SubItemModel>();
                return new ResponseModel
                {
                    Code = 1,
                    Message = "Success",
                    Data = new HomeModel { Items = itemsModel ,SubItems =SubItemModel},
                };
            }
        }
    }
}
