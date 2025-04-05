# Result Pattern Implementation 🚀

A simple and elegant implementation of the **Result pattern** in C# for robust error handling. Perfect for clean architecture and domain-driven design (DDD) projects.

![Result Pattern](https://img.shields.io/badge/C%23-10-blueviolet) 
![License](https://img.shields.io/badge/License-MIT-green)

## Features ✨
- 🟢 **Success/Failure** states with `IsSuccess`/`IsFailure` flags
- 🔒 **Immutable** results (except for `Error` setter)
- 🎯 **Generic variant** `Result<TValue>` for returning values
- ⚡ **Implicit operators** for quick creation
- 🛡️ **Validation** in constructors to prevent invalid states

## Installation 📦
Add directly to your project:
```csharp
// Copy both classes to your project's Core/Helpers folder
public class Result { ... }
public class Result<TValue> : Result { ... }

## Usage 🛠️


### Data validation (ASP.NET Core Controller)
```csharp

public Result<User> RegisterUser(RegisterRequest request)
{
 
    if (!IsValidEmail(request.Email))
        return "Invalid email format";  
 
    var existingUser = _userRepository.FindByEmail(request.Email);
    if (existingUser != null)
        return Result<User>.Falure("Email already taken");

 
    var user = new User(request.Email, request.Password);
    _userRepository.Add(user);
    
    return user; // implicit success (Result<User>)
}

 
[HttpPost]
public IActionResult Register([FromBody] RegisterRequest request)
{
    var result = RegisterUser(request);
    
    return result.IsSuccess 
        ? Ok(result.Value) 
        : BadRequest(result.Error);
}

```

### Working with the database (Repository Pattern)






```csharp
public Result<Invoice> GetInvoiceById(int id)
{
    try
    {
        var invoice = _context.Invoices
            .Include(i => i.Items)
            .FirstOrDefault(i => i.Id == id);

        return invoice != null 
            ? invoice 
            : Result<Invoice>.Falure("Invoice not found");
    }
    catch (SqlException ex)
    {
        return $"Database error: {ex.Message}"; // implicit failure
    }
}

 
public Result ProcessPayment(int invoiceId)
{
    var invoiceResult = GetInvoiceById(invoiceId);
    if (invoiceResult.IsFalure)
        return invoiceResult.Error!;

    return _paymentService.Process(invoiceResult.Value);
}

```
