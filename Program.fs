// Learn more about F# at http://fsharp.org

open System
open MyErrorTuple

[<EntryPoint>]
let main argv = 

    let hello x = 
        printfn "Hello %d" x
        x + 1

    let error x = 
        printfn "Error %d" x
        raise (Exception "Error!")
        x + 1
    
    let world x = 
        printfn "World %d" x
        x + 1
    
    let mutable r1 = 0
    try 
        r1 <- hello 0
    with ex -> 
        printfn "%A" ex 
    
    try 
        r1 <- error r1
    with ex -> 
        printfn "%A" ex

    try 
        r1 <- world r1
    with ex -> 
        printfn "%A" ex
    
    printfn "r1:%A" r1

    let execute f x =
        try 
            f x
        with ex -> 
            printfn "error:%A" ex
            0

    let r2 = execute hello 0
    let r3 = execute error r2
    let r4 = execute world r3
    
    printfn "r2:%A r3:%A r4:%A" r2 r3 r4

    let helloErrorWorld = 
        hello >> error >> world
    
    let r5 = execute helloErrorWorld 0
    printfn "r5:%A" r5

    let helloWorld = 
        hello >> world
    let r6 = execute helloWorld 0
    printfn "r6:%A" r6

    let r8 = MyErrorTuple.hello 0
    printfn "r8:%A" r8
    let r9 = MyErrorTuple.bindSuccess  MyErrorTuple.hello r8
    printfn "r9:%A" r9
    let r10 = MyErrorTuple.bindSuccess  MyErrorTuple.hello r9
    printfn "r10:%A" r10

    let r11 = MyErrorTuple.hello 1
    printfn "r11:%A" r11
    let r12 = MyErrorTuple.bindFail  MyErrorTuple.handle r11
    printfn "r12:%A" r12
    let r13 = MyErrorTuple.bindSuccess  MyErrorTuple.hello r12
    printfn "r13:%A" r13

    let r14 = MyErrorTuple.helloHelloHello 0
    printfn "r14:%A" r14

    let r15 = MyErrorTuple.helloHelloHandle 0
    printfn "r15:%A" r15

    let r16 = MyErrorTuple.helloHandleHelloHandle 1
    printfn "r16:%A" r16

    let r17 = MyErrorTuple.helloHandleHelloHandle 1
    printfn "r17:%A" r17

    0 // return an integer exit code
