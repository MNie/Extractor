namespace Extractor.Core

open FSharp.Text.RegexProvider
open System.Collections.Generic
open System

type someRegex = Regex< @"(?<Hit>^\{FROM_SOME:(.*)\})" >

type ValuesProvider() =
    let (|IsSome|_|) (value: string) =
        let parsedValue = someRegex().TypedMatch(value).``1``.Value
        if parsedValue |> String.IsNullOrWhiteSpace then None
        else Some parsedValue

    let getValueFromProperties key (properties: IDictionary<string, string>) =
        let (|IsInsideProperties|_|) (value) =
            if properties.ContainsKey(value) then Some value
            else None

        match key with
        | IsInsideProperties r -> properties.[key]
        | _ -> key

    let extractSome(value: string) =
        let matched = someRegex().TypedMatch(value).``1``.Value

        if matched |> String.IsNullOrWhiteSpace then getValueFromProperties value
        else getValueFromProperties matched

    let rec getValue value properties =
        match value with
        | IsSome r ->
            let extractedSome = extractSome r properties
            let parsedValue = getValue extractedSome
            parsedValue properties
        | _ -> getValueFromProperties value properties

    member this.Provide(value, properties) =
        getValue value properties
