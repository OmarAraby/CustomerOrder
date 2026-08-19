using CustomerOrder.Application.Dtos.Orders;
using FluentValidation;
using System;
using System.Linq;

namespace CustomerOrder.Application.Validators
{
    public class UpdateOrderDtoValidator : AbstractValidator<UpdateOrderDto>
    {
        public UpdateOrderDtoValidator()
        {
            RuleFor(dto => dto.OrderNumber)
                .NotEmpty().WithMessage("Order number is required.")
                .MaximumLength(30).WithMessage("Order number must not exceed 30 characters.")
                .Matches(@"^ORD-[0-9]{6}$").WithMessage("Order number must look like ORD-000123.");

            RuleFor(dto => dto.OrderDate)
                .Must(date => date != default(DateTime)).WithMessage("Order date is required.")
                .Must(date => date <= DateTime.UtcNow).WithMessage("Order date cannot be in the future.");

            RuleFor(dto => dto.TotalAmount)
                .GreaterThan(0m).WithMessage("Total amount must be greater than zero.");

            RuleFor(dto => dto.Status)
                .IsInEnum().WithMessage("Status is not a valid order status.");

            RuleFor(dto => dto.CustomerIds)
                .NotNull().WithMessage("At least one customer is required.")
                .Must(ids => ids != null && ids.Count > 0)
                    .WithMessage("An order must be linked to at least one customer.")
                .Must(ids => ids == null || ids.All(id => id > 0))
                    .WithMessage("Customer ids must be positive.")
                .Must(ids => ids == null || ids.Distinct().Count() == ids.Count)
                    .WithMessage("Customer ids must not repeat.");
        }
    }
}