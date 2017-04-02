module Tests

open Expecto
open System.Collections.Generic
open Extractor.Core

[<Tests>]
let tests =
  let value = "45"
  let dependFunctionName = "dee"
  let baseFunctionName = "dede"
  let inputCreator parameter = sprintf "{FROM_SOME:%s}" parameter
  let valuesProvider = ValuesProvider()
  testList "samples" [
    testCase "simple value" <| fun _ ->
      let input = "45"
      let result = (input, dict []) |> valuesProvider.Provide
      Expect.equal result input "result from values provider should be equal to 45"

    testCase "value with single" <| fun _ ->
      let input = baseFunctionName |> inputCreator
      let properties = dict [(baseFunctionName, value)]
      let result = (input, properties) |> valuesProvider.Provide
      Expect.equal result value "result from values provider should be equal to 45"

    testCase "value with multiple dependency" <| fun _ ->
      let input = dependFunctionName |> inputCreator |> inputCreator
      let properties = dict [ (dependFunctionName, baseFunctionName); (baseFunctionName, value)]
      let result = (input, properties) |> valuesProvider.Provide
      Expect.equal result value "result from values provider should be equal to 45"
  ]