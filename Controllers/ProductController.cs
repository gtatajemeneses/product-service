using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProductService.Common;
using ProductService.Dtos;
using ProductService.Entities;
using ProductService.Repositories;

namespace ProductService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<ProductRequestDto> _validator;
    public ProductController(IProductRepository repository,
     IMapper mapper, IValidator<ProductRequestDto> validator)
    {
        _mapper = mapper;
        _repository = repository;
        _validator = validator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> Get()
    {
        var products = await _repository.GetAllAsync();
        var productsDto = _mapper.Map<IEnumerable<ProductResponseDto>>(products);

        return Ok(new ApiResponse<IEnumerable<ProductResponseDto>>
        {
            Data = productsDto,
            Message = "Operación realizada con éxito",
            StatusCode = 200
        });
    }
    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> Create(ProductRequestDto productRequestDto)
    {
        var validationResult = await _validator.ValidateAsync(productRequestDto);
        // Extraemos los mensajes de error
        var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

        if (!validationResult.IsValid)
        {
            return BadRequest(new ApiResponse<List<string>>
            {
                Data = errors,
                Message = "Validation error",
                StatusCode = 400
            });
        }

        var product = await _repository.CreateAsync(_mapper.Map<Product>(productRequestDto));
        var productResponseDto = _mapper.Map<ProductResponseDto>(product);
        return Ok(new ApiResponse<ProductResponseDto>
        {
            Data = productResponseDto,
            Message = "Operación realizada con éxito",
            StatusCode = 200
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProductResponseDto>>> GetById(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound(new ApiResponse<string>
            {
                Data = "",
                Message = "Product Not found",
                StatusCode = 404
            });
        }
        var productResponseDto = _mapper.Map<ProductResponseDto>(product);
        return Ok(new ApiResponse<ProductResponseDto>
        {
            Data = productResponseDto,
            Message = "Success",
            StatusCode = 200
        });
    }

    [HttpPut]
    public async Task<ActionResult<ApiResponse<ProductResponseDto>>> Update(ProductRequestDto productRequestDto)
    {
        var product = await _repository.GetByIdAsync(productRequestDto.Id);
        if (product == null)
        {
            return NotFound(new ApiResponse<string>
            {
                Data = "",
                Message = "Product not found",
                StatusCode = 404
            });
        }

        var validationResult = await _validator.ValidateAsync(productRequestDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ApiResponse<string>
            {
                Data = "",
                Message = validationResult.ToString(),
                StatusCode = 400
            });
        }

       await _repository.UpdateAsync(_mapper.Map(productRequestDto, product));

        var productResponseDto = _mapper.Map<ProductResponseDto>(product);
        return Ok(new ApiResponse<ProductResponseDto>
        {
            Data = productResponseDto,
            Message = "Success",
            StatusCode = 200
        });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var deleted = await _repository.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new ApiResponse<bool>
            {
                Data = false,
                Message = $"El producto con ID {id} no existe.",
                StatusCode = 404,
                Success = false
            });
        }

        // Retornamos 200 OK envolviendo la confirmación en tu clase estándar
        return Ok(new ApiResponse<bool>
        {
            Data = true,
            Message = "Producto eliminado con éxito",
            StatusCode = 200,
            Success = true
        });
    }

}
