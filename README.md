# Reed-Solomon-encoder-and-decoder
This is an implementation of an R-S encoder and decoder in C#.

Advantage: 

You can generate your own R-S code based on two parameteres: 

-the number of bits that will make out your symbols (m)

-the number of symbols you want your R-S code be able to correct (t)

Limitations:

-maximum number of "m" is 8 => you can change this by adding more primitive polynomials by going to the Encoder class -> GetPrimitivePolynomial method OR by creating your own method that calculates primitive polynomials using any m > 2 (better option) 

-the decoder knows exactly how many errors happened on the codeword during transmission (this is ofcourse, not happening in real life), the methods and algorithms I used on decoder require me to know the number of errors beforehand

-the maximum number of errors my code can correct is 3 unfortunatelly.

  I used "Linear recursion method for σ(X)" and "Chien search algorithm" for finding error locations 
  and "the direct solution method" for finding error values. 
  
  Basically, when encountering 4 erros my code cannot find the determinant of a 4x4 matrix because it contains dependent equations that prevent the calculation of an inverse matrix which is always resulting in a zero 
  determinant.
  
  Any number of errors above 4 use the 4x4 matrix, therefore they are also impossible to calculate.



I hope that, even though my R-S code isn't the best it will help you in your R-S encoding and decoding journey! :')
