module GitHubActionMain

open System
open System.IO
open System.Text.RegularExpressions
open Newtonsoft.Json

let GITHUB_OUTPUT_FILE = "GITHUB_OUTPUT"

let private removeSquareBrackets (s: string): string =
    Regex.Replace(s, "\[|\]", "")

let private removeDoubleQuotes (s: string): string =
    s.Replace("\"", "")

let private removeBackslash (s: string): string =
    s.Replace("\\", "")

let getDirectory (s: string): string =
    let dirsNameArray = s.Split [| '/' |]

    match (dirsNameArray.Length > 2) with
    | true -> $"{dirsNameArray.[0]}/{dirsNameArray.[1]}"
    | false -> ""

let getFormattedDirectories (args: string array) =
    (args.[0]
     |> removeSquareBrackets
     |> removeDoubleQuotes
     |> removeBackslash).Split [| ',' |]
     |> Array.map getDirectory
     |> Array.distinct

let getserializedResult (result: string array): string =
    JsonConvert.SerializeObject result

let private getGithubOutputPath (): string =
    Environment.GetEnvironmentVariable(GITHUB_OUTPUT_FILE)

// https://docs.github.com/en/actions/creating-actions/creating-a-docker-container-action#writing-the-action-code
let private writeToGithubOutput (serializedResult: string): unit =
    let githubOutputPath = getGithubOutputPath()
    printfn "githubOutputPath: %s" githubOutputPath

    match (File.Exists githubOutputPath) with
    | false -> () // when running outside Github Actions environment
    | true ->
        File.AppendAllText(githubOutputPath, $"dirschanged={serializedResult}")

[<EntryPoint>]
let main (args: string array): int =
    let result = getFormattedDirectories args
    let serializedResult = getserializedResult result
    printfn "serializedResult: %s" serializedResult

    writeToGithubOutput serializedResult

    0 // no error code
