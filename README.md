# STDISCM-Threaded-Prime-Number-Search

This project serves as partial fulfillment of the STDISCM Course

Nicole Ong S11


## Description
A prime number checker that uses threads created in C#, that will create an x number of threads that will search for prime numbers to a given y number. To configure the parameters for the number of threads, upper limit, printing variations and task divisions, set them in config.txt. By default, the program will use the following specifications:

```
NumberOfThreads=4
UpperLimit=1000
Printing=immediate
DivisionScheme=Straight
```

To run the program, use Run/Debug in an IDE such as Visual Studio.

### NumberOfThreads

The number of threads that will search for prime numbers. Set to any integer. If input is not valid, it will default to 4.

### UpperLimit

The limit of where the program will search for prime numbers. Set to any integer. If input is not valid, it will default to 1000.

### Printing

The setting for printing variations. Has two modes:  
`immediate`(default): will print logs (primes/divisors, thread id and time stamp) as soon as they are run  
`wait`: will wait until all threads are done then print everything

### Division Scheme

The setting for how tasks will be divided among threads. Has two modes:  
`straight`(default): Straight division of search range. (ie for 1 - 1000 and 4 threads the division will be 1-250, 251-500, and so forth)  
`linear`: The search is linear but the threads are for divisibility testing of individual numbers.
