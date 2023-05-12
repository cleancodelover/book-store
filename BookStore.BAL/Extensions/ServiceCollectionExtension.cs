using BookStore.BLL.BusinessLogic;
using BookStore.BLL.BusinessLogic.Interfaces;
using BookStore.DAL.Contracts;
using BookStore.DAL.Models;
using BookStore.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.BLL.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBLLDependencies(this IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServer();
            services.AddScoped<IRepository<AspNetUser>, UserRepository>();
            services.AddScoped<IRepository<Wallet>, WalletRepository>();
            services.AddScoped<IRepository<Book>, BookRepository>();
            services.AddScoped<IRepository<Review>, ReviewRepository>();
            services.AddScoped<IRepository<Cart>, CartRepository>();
            services.AddScoped<IRepository<CartItem>, CartItemRepository>();
            services.AddScoped<IRepository<Transaction>, TransactionRepository>();
            services.AddScoped<IUserBL, UserBL>();
            services.AddScoped<IWalletBL, WalletBL>();
            services.AddScoped<IBookBL, BookBL>();
            services.AddScoped<IReviewBL, ReviewBL>();
            services.AddScoped<ICartBL, CartBL>();
            services.AddScoped<ICartItemBL, CartItemBL>();
            services.AddScoped<ITransactionBL, TransactionBL>();
            return services;
        }
    }
}
