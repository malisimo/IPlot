module Utils

let firstCharToLower (str:string) =
    match str.Length with
    | 0 -> ""
    | 1 -> str.ToLower()
    | _ -> 
        System.Char.ToLower str.[0]
        |> fun x -> string x + str.Substring 1

let firstCharToUpper (str:string) =
    match str.Length with
    | 0 -> ""
    | 1 -> str.ToUpper()
    | _ -> 
        System.Char.ToUpper str.[0]
        |> fun x -> string x + str.Substring 1

let makeSafeTypeName name =
    let unHyphen (str:string) =
        str.Replace("-","_")
        
    match name with
    | "type" -> "iplot_type"
    | "end" -> "iplot_end"
    | "base" -> "iplot_base"
    | "default" -> "iplot_default"
    | "params" -> "iplot_params"
    | "operator" -> "iplot_operator"
    | name -> name
    |> unHyphen

let pathToPropName (path: string list) =
    path
    |> List.map firstCharToUpper
    |> (fun l -> "IProp"::l)
    |> List.rev
    |> String.concat "_"