using AutoMapper;
using System;
using WebSurveying2017.Model.Model;
using WebSurveying2017.ViewModels;

namespace WebSurveying2017.Mapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        public DomainToViewModelMappingProfile()
        {
            CreateMap<Survey, SurveyViewModel>();

            CreateMap<Survey, SurveyResultViewModel>();

            CreateMap<Question, QuestionResultVM>().ForMember(dest => dest.NumbOfUsers,
                opt => opt.Ignore());
            CreateMap<Question, QuestionResultVM>().ForMember(dest => dest.NumbOfUsers,
                opt => opt.Ignore());
            CreateMap<Answer, UsersAnswersVM>().ForMember(dest => dest.Count,
                opt => opt.Ignore());
            CreateMap<Answer, UsersAnswersVM>().ForMember(dest => dest.IsAnswerOfUser,
                opt => opt.Ignore());
            CreateMap<Category, CategoryViewModel>();
            CreateMap<Category, CategoriesViewModel>();
            CreateMap<Comment, CommentViewModel>();
            CreateMap<ApplicationUser, UserViewModel>();
            CreateMap<SurveyGroup, SurveyGroupViewModel>();
            CreateMap<Group, GroupViewModel>();
            CreateMap<Comment, CommentViewModel>().ForMember(dest => dest.User,
                opt => opt.Ignore());


        }

    }
}