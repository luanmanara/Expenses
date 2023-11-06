using AutoMapper;
using ExpensesAPI.Models;
using ExpensesAPI.Models.Dto;

namespace ExpensesAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            // Wallet mapping
            CreateMap<Wallet, WalletDTO>().ReverseMap();
            CreateMap<Wallet, WalletCreateDTO>().ReverseMap();
            CreateMap<Wallet, WalletUpdateDTO>().ReverseMap().ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));

            // Period mapping
            CreateMap<Period, PeriodDTO>().ReverseMap();
            CreateMap<Period, PeriodCreateDTO>().ReverseMap();
            CreateMap<Period, PeriodUpdateDTO>().ReverseMap().ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));

            // Transaction mapping
            CreateMap<Transaction, TransactionDTO>().ReverseMap();
            CreateMap<Transaction, TransactionCreateDTO>().ReverseMap();
            CreateMap<Transaction, TransactionUpdateDTO>().ReverseMap().ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));
        }
    }
}
