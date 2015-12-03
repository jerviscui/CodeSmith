using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Runtime.Session;

namespace CodeSmith.Abp.Template.Test.Application
{
    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TSelectRequestInput, in TCreateInput, in TUpdateInput>
        : IApplicationService
        where TSelectRequestInput : IPagedResultRequest
    {
    
        TEntityDto Get(IdInput<TPrimaryKey> input);

        PagedResultOutput<TEntityDto> GetAll(TSelectRequestInput input);

        Task<TPrimaryKey> CreateAsync(TCreateInput input);

        TPrimaryKey Create(TCreateInput input);

        void Update(TUpdateInput input);

        void Delete(IdInput<TPrimaryKey> input);
    }

    public interface ICrudAppService<TEntityDto, TPrimaryKey, in TSelectRequestInput>
        : ICrudAppService<TEntityDto, TPrimaryKey, TSelectRequestInput, TEntityDto, TEntityDto>
        where TSelectRequestInput : IPagedResultRequest
    {

    }

    public interface ICrudAppService<TEntityDto, TPrimaryKey>
        : ICrudAppService<TEntityDto, TPrimaryKey, DefaultPagedResultRequest>
    {

    }

    public interface ICrudAppService<TEntityDto>
        : ICrudAppService<TEntityDto, int>
    {

    }
}