namespace YuyoDev.Application.DTOs.Products;

using FluentValidation;

public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del producto es obligatorio.")
            .MaximumLength(150).WithMessage("El nombre no puede superar los 150 caracteres.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Debe asignar una categoría al producto.");

        RuleFor(x => x.BrandId)
            .NotEmpty().WithMessage("Debe asignar una marca al producto.");
    }
}