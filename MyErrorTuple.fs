module MyErrorTuple

    open System

    type Error = 
        | HelloError
        | WorldError

    type Result<'a, 'b> = Choice<'a, Error * 'b>
    
    let success (x : 'a) : Result<'a,_> = Choice1Of2 x

    let fail (err : Error * 'a) : Result<_,'a> = Choice2Of2 err

    let bindSuccess (fCont : 'a -> Result<'b,_>) (value : Result<'a,_>) : Result<'b,_> =
        match value with
        | Choice1Of2 x -> fCont x
        | Choice2Of2 err -> Choice2Of2 err

    let bindFail (fCont : Error * 'a -> Result<_,'b>) (value : Result<_,'a>) : Result<_,'b> =
        match value with
        | Choice1Of2 x -> Choice1Of2 x
        | Choice2Of2 err -> fCont err

    let composeSuccess (first : 'a -> Result<'b,_>) (second : 'b -> Result<'c,_>) =
        fun x ->
            bindSuccess second (first x)
    
    let composeFail (first : Error * 'a -> Result<_,'b>) (second : Error * 'b -> Result<_,'c>) =
        fun x ->
            bindFail second (first x)
    let (>==>) a b =
        composeSuccess a b

    let (>**>) a b =
        composeFail a b

    let composeSuccessFail (first : 'a -> Result<'b,'a>) (second : Error * 'a -> Result<'b,'c>) =
        fun x ->
            bindFail second (first x)
    let (>=*>) a b =
        composeSuccessFail a b

    let composeFailSuccess (first : Error * 'a -> Result<'b,'c>) (second : 'b -> Result<'d,'c>) =
        fun x ->
            bindSuccess second (first x)
    let (>*=>) a b =
        composeFailSuccess a b

    let hello x = 
        printfn "Hello %d" x
        if x % 2 = 0 then
            success (x + 1)
        else
            fail (HelloError, x)

    let world x = 
        printfn "Hello %d" x
        if x % 2 = 0 then
            success (x + 1)
        else
            fail (WorldError, x)

    let handle x = 
        match x with
        | (HelloError, value) ->  printfn "Handle %d" value
                                  hello (value + 1)
        | (_,_) -> fail x

    let helloHelloHello = hello >==> hello >==> hello
    let handleHandleHandle = handle >**> handle >**> handle

    let helloHelloHandle = hello >==> hello >=*> handle
    let handleHelloHello = handle >*=> hello >==> hello

    let helloHandleHello = hello >=*> handle >==> hello
    let handleHelloHandle = handle >*=> hello >**> handle

    let helloHandle = hello >=*> handle
    let helloHandleHelloHandle = helloHandle >==> helloHandle