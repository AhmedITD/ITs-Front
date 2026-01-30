//==============================================================================
// FILE: Add.cs
// LOCATION: src/Services/Operations/Add.cs
// PURPOSE: Addition operation for scientific calculator
//
// 📚 LEARNING OBJECTIVES:
//   ✅ Understand how Strategy pattern works in practice
//   ✅ Learn to implement an interface (IOperation)
//   ✅ Practice using LINQ methods (Sum, Length, etc.)
//   ✅ Understand params keyword for variable arguments
//
// 📋 PREREQUISITES:
//   ✅ Know what interfaces are and how to implement them
//   ✅ Understand basic LINQ (at least the Sum method)
//   ✅ Familiar with arrays and collections
//
// ⭐ DIFFICULTY: Easy (Perfect for beginners!)
// ⏱️ ESTIMATED TIME: 10-15 minutes
//
// 🧪 TEST CASES (use these to verify your implementation):
//   Input: add 2 3          → Expected: 5.0
//   Input: add 1 2 3 4      → Expected: 10.0
//   Input: add              → Expected: 0.0 (no arguments)
//   Input: add 5            → Expected: 5.0 (single argument)
//   Input: add -5 5         → Expected: 0.0 (negative numbers)
//   Input: add 0.5 0.5 0.5  → Expected: 1.5 (decimals)
//==============================================================================

using System;
using System.Linq;
using SciCalc.Models;

namespace SciCalc.Services.Operations;

/// <summary>
/// Addition operation - sums multiple numbers.
/// Example: add 1 2 3 → 6
/// Used for basic arithmetic and can handle any number of arguments.
/// </summary>
public sealed class Add : IOperation
{
    /// <summary>
    /// Name of this operation - used when user types commands.
    /// </summary>
    public string Name => "add";
    
    /// <summary>
    /// Help text shown to users.
    /// </summary>
    public string Help => "add x y [z ...] - Adds numbers together";
    
    /// <summary>
    /// ==================== STUDENT TODO ====================
    /// Implement addition of variable arguments.
    /// 
    /// 📝 REQUIREMENTS:
    /// 1. Handle 0 arguments → return 0
    /// 2. Handle 1 argument → return that number
    /// 3. Handle multiple arguments → sum them all
    /// 
    /// 💡 HINTS:
    /// - You can use LINQ's Sum() method: args.Sum()
    /// - Check array length with: args.Length
    /// - Handle empty case: if (args.Length == 0) return 0;
    /// - Or use ternary: return args.Length == 0 ? 0 : args.Sum();
    /// 
    /// 📖 HELPFUL LINKS:
    /// - LINQ Sum: https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.sum
    /// - params keyword: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/params
    /// 
    /// ⚡ COMPLEXITY ANALYSIS:
    /// - Time Complexity: O(n) where n = number of arguments
    /// - Space Complexity: O(1) - only using constant extra space
    /// 
    /// 🔍 STEP-BY-STEP GUIDE:
    /// Step 1: Check if args array is empty
    ///         if (args.Length == 0) return 0;
    /// 
    /// Step 2: Use LINQ Sum() to add all numbers
    ///         return args.Sum();
    /// 
    /// Step 3: Test with the test cases above!
    /// 
    /// 🎯 ONE-LINE SOLUTION (if you want a challenge):
    /// You can solve this in a single return statement using the ternary operator!
    /// =======================================================
    /// </summary>
    /// <param name="args">Numbers to add together</param>
    /// <returns>Sum of all arguments, or 0 if no arguments provided</returns>
    public double Evaluate(params double[] args)
    {
        // TODO: Remove this line and implement your solution
        throw new NotImplementedException(
            "STUDENT TODO: Implement addition logic. " +
            "Hint: Check if array is empty, then use LINQ Sum() method."
        );
        
        // Example structure (remove comments and fill in):
        // if (/* check if empty */)
        //     return /* what to return for empty? */;
        // 
        // return /* sum of all args */;
    }
}

/*
 * 🎓 LEARNING NOTES:
 * 
 * Why use params keyword?
 * - Allows calling: Evaluate(1, 2, 3) instead of Evaluate(new[] {1, 2, 3})
 * - Makes the method more user-friendly
 * - C# compiler converts it to array automatically
 * 
 * Why LINQ Sum()?
 * - Clean and readable: args.Sum() vs manual loop
 * - Built-in, well-tested implementation
 * - Follows functional programming style
 * 
 * Alternative implementations:
 * 1. LINQ (recommended): args.Sum()
 * 2. Loop: foreach(var x in args) sum += x;
 * 3. For loop: for(int i=0; i<args.Length; i++) sum += args[i];
 * 4. Recursion: (not recommended for this simple case)
 * 
 * Design Pattern Context:
 * - This class is part of the STRATEGY pattern
 * - Each operation (Add, Sub, Mul, etc.) is a different strategy
 * - OperationFactory creates the right strategy based on command
 * - Calculator uses the strategy without knowing which one it is
 */
