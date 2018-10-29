using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.BindingModels;
using WebSurveying2017.BindModels;
using WebSurveying2017.Model.Model;
using WebSurveying2017.Models;
using WebSurveying2017.ViewModels;

namespace WebSurveying2017.Mapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        public ViewModelToDomainMappingProfile()
        {
            CreateMap<SurveyViewModel, Survey>();

            CreateMap<SurveyResultViewModel, Survey>();

            CreateMap<SurveyBindingModel, Survey>();

            CreateMap<QuestionBindingModel, Question>();

            CreateMap<AnswerBindingModel, Answer>();

            CreateMap<CategoryBindingModel, Category>();

            CreateMap<UserAnswerBindingModel, UserAnswer>();

            CreateMap<CommentBindingModel, Comment>();

            CreateMap<UserGroupBindingModel, UserGroup>();

            CreateMap<SurveyGroupBindingMModel, SurveyGroup>();

            CreateMap<UpdateUserBindingModel, ApplicationUser>();

            CreateMap<GroupBindingModel, Group>();
        }
    }
}