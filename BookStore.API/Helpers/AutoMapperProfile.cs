using AutoMapper;
using BookStore.API.ViewModels.Post;
using BookStore.API.ViewModels.Get;
using BookStore.API.ViewModels.Put;
using BookStore.DAL.Models;

namespace BookStore.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateAspNetUserVM, AspNetUser>();
            CreateMap<AspNetUser, CreateAspNetUserVM>();
            CreateMap<CreateAspNetRoleClaimVM, AspNetRoleClaim>();
            CreateMap<AspNetRoleClaim, CreateAspNetRoleClaimVM>();
            CreateMap<CreateAspNetRoleVM, AspNetRole>();
            CreateMap<AspNetRole, CreateAspNetRoleVM>();
            CreateMap<CreateAspNetUserClaimVM, AspNetUserClaim>();
            CreateMap<AspNetUserClaim, CreateAspNetUserClaimVM>();
            CreateMap<CreateAspNetUserLoginVM, AspNetUserLogin>();
            CreateMap<AspNetUserLogin, CreateAspNetUserLoginVM>();
            CreateMap<CreateAspNetUserTokenVM, AspNetUserToken>();
            CreateMap<AspNetUserToken, CreateAspNetUserTokenVM>();
            CreateMap<CreateBookVM, Book>();
            CreateMap<Book, CreateBookVM>();
            CreateMap<CreateCartItemVM, CartItem>();
            CreateMap<CartItem, CreateCartItemVM>();
            CreateMap<CreateCartVM, Cart>();
            CreateMap<Cart, CreateCartVM>();
            CreateMap<CreateReviewVM, Review>();
            CreateMap<Review, CreateReviewVM>();
            CreateMap<CreateTransactionVM, Transaction>();
            CreateMap<Transaction, CreateTransactionVM>();
            CreateMap<CreateWalletVM, Wallet>();
            CreateMap<Wallet, CreateWalletVM>();


            CreateMap<EditAspNetUserVM, AspNetUser>();
            CreateMap<AspNetUser, EditAspNetUserVM>();
            CreateMap<EditAspNetRoleClaimVM, AspNetRoleClaim>();
            CreateMap<AspNetRoleClaim, EditAspNetRoleClaimVM>();
            CreateMap<EditAspNetRoleVM, AspNetRole>();
            CreateMap<AspNetRole, EditAspNetRoleVM>();
            CreateMap<EditAspNetUserClaimVM, AspNetUserClaim>();
            CreateMap<AspNetUserClaim, EditAspNetUserClaimVM>();
            CreateMap<EditAspNetUserLoginVM, AspNetUserLogin>();
            CreateMap<AspNetUserLogin, EditAspNetUserLoginVM>();
            CreateMap<EditAspNetUserTokenVM, AspNetUserToken>();
            CreateMap<AspNetUserToken, EditAspNetUserTokenVM>();
            CreateMap<EditBookVM, Book>();
            CreateMap<Book, EditBookVM>();
            CreateMap<EditCartItemVM, CartItem>();
            CreateMap<CartItem, EditCartItemVM>();
            CreateMap<EditCartVM, Cart>();
            CreateMap<Cart, EditCartVM>();
            CreateMap<EditReviewVM, Review>();
            CreateMap<Review, EditReviewVM>();
            CreateMap<EditTransactionVM, Transaction>();
            CreateMap<Transaction, EditTransactionVM>();
            CreateMap<EditWalletVM, Wallet>();
            CreateMap<Wallet, EditWalletVM>();


            CreateMap<AspNetUserVM, AspNetUser>();
            CreateMap<AspNetUser, AspNetUserVM>();
            CreateMap<AspNetRoleClaimVM, AspNetRoleClaim>();
            CreateMap<AspNetRoleClaim, AspNetRoleClaimVM>();
            CreateMap<AspNetRoleVM, AspNetRole>();
            CreateMap<AspNetRole, AspNetRoleVM>();
            CreateMap<AspNetUserClaimVM, AspNetUserClaim>();
            CreateMap<AspNetUserClaim, AspNetUserClaimVM>();
            CreateMap<AspNetUserLoginVM, AspNetUserLogin>();
            CreateMap<AspNetUserLogin, AspNetUserLoginVM>();
            CreateMap<AspNetUserTokenVM, AspNetUserToken>();
            CreateMap<AspNetUserToken, AspNetUserTokenVM>();
            CreateMap<BookVM, Book>();
            CreateMap<Book, BookVM>();
            CreateMap<CartItemVM, CartItem>();
            CreateMap<CartItem, CartItemVM>();
            CreateMap<CartVM, Cart>();
            CreateMap<Cart, CartVM>();
            CreateMap<ReviewVM, Review>();
            CreateMap<Review, ReviewVM>();
            CreateMap<TransactionVM, Transaction>();
            CreateMap<Transaction, TransactionVM>();
            CreateMap<WalletVM, Wallet>();
            CreateMap<Wallet, WalletVM>();
        }
    }
}
