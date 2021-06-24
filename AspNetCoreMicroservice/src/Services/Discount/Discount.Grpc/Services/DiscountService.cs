﻿using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository repository;
        private readonly IMapper mapper;
        private readonly ILogger<DiscountService> logger;

        public DiscountService(IDiscountRepository repository, IMapper mapper, ILogger<DiscountService> logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await repository.GetDiscount(request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
            }
            logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

            var couponModel = mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = mapper.Map<Coupon>(request.Coupon);

            await repository.CreateDiscount(coupon);
            logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", coupon.ProductName);

            var couponModel = mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = mapper.Map<Coupon>(request.Coupon);

            await repository.UpdateDiscount(coupon);
            logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", coupon.ProductName);

            var couponModel = mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await repository.DeleteDiscount(request.ProductName);
            var response = new DeleteDiscountResponse
            {
                Success = deleted
            };

            return response;
        }
    }
}
