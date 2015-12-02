using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;


namespace CodeSmith.Abp.Template.Test.Application
{
    public class Dto : IEntityDto<int>, IInputDto, IOutputDto
    {
        public void Meth()
        {

        }

        public int Id { get; set; }
    }
}
