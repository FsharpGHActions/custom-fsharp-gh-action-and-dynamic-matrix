module GitHubActionTests

open Expecto
open GitHubActionMain

let zeroDirZeroSize = "[]"
let oneDirOneSize = "[\"terraform/instance-1/network/main.tf\"]"
let oneDirTwoSize = "[\"terraform/instance-1/network/main.tf\",\"terraform/instance-1/network/outputs.tf\"]"
let twoDirThreeSizeArray = "[\"terraform/instance-1/network/main.tf\",\"terraform/instance-1/network/outputs.tf\",\"terraform/instance-2/network/main.tf\"]"

[<Tests>]
let tests =
    testList "Different input scenarios" [
        test "Empty array input" {
            let result = getFormattedDirectories [| zeroDirZeroSize |]
            Expect.equal result [| "" |] "The strings should equal"

            let serializedResult = getserializedResult result
            Expect.equal serializedResult "[\"\"]" "The strings should equal"
        }

        test "Single directory one-size array input" {
            let result = getFormattedDirectories [| oneDirOneSize |]
            Expect.equal result [| "terraform/instance-1" |] "The strings should equal"

            let serializedResult = getserializedResult result
            Expect.equal serializedResult "[\"terraform/instance-1\"]" "The strings should equal"
        }

        test "Single directory two-size array input" {
            let result = getFormattedDirectories [| oneDirTwoSize |]
            Expect.equal result [| "terraform/instance-1" |] "The strings should equal"

            let serializedResult = getserializedResult result
            Expect.equal serializedResult "[\"terraform/instance-1\"]" "The strings should equal"
        }

        test "Two directories three-size array input" {
            let result = getFormattedDirectories [| twoDirThreeSizeArray |]
            Expect.equal result [| "terraform/instance-1"; "terraform/instance-2" |] "The strings should equal"

            let serializedResult = getserializedResult result
            Expect.equal serializedResult "[\"terraform/instance-1\",\"terraform/instance-2\"]" "The strings should equal"
        }
    ]

[<EntryPoint>]
let main args =
    runTests defaultConfig tests
