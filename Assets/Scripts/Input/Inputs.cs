// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Input/Inputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace FinalInferno.Input
{
    public class @Inputs : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @Inputs()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Inputs"",
    ""maps"": [
        {
            ""name"": ""Overworld"",
            ""id"": ""014c8677-432d-4386-8b29-1b983ddc567b"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""09713dd2-8cbb-4c20-97b8-e2f247373216"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""bde54938-b6b0-481b-9e0e-0962c7958235"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint / Skip Dialogue"",
                    ""type"": ""Button"",
                    ""id"": ""739b948f-82df-44c9-8b81-7e7cf2479fae"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Open Menu"",
                    ""type"": ""Button"",
                    ""id"": ""249fd483-2f35-41ce-9c4e-2d1d957a36d6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Repel Skill"",
                    ""type"": ""Button"",
                    ""id"": ""38124da0-080c-4ca4-9bd4-44dcc8dccc2c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Taunt Skill"",
                    ""type"": ""Button"",
                    ""id"": ""fbe93c03-abf3-4e4e-a7f4-1110a58e1221"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""583fde2a-79fd-4025-9445-ed164afac7b8"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3f30af2b-c113-496c-9853-fd7244e54a66"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a7c4c7e3-6f17-471e-bab9-08fe115b7e7f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d67410d3-8a0b-4b62-9dfb-4147214fa547"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6eb5d997-cdf2-461e-b44f-58b0cb8014e8"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow Keys"",
                    ""id"": ""00381269-873c-4cee-af43-c99bd80f092f"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9c1afd00-9f1b-4fcd-ac89-0fae11232d50"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f2ee4746-546a-4c1e-8536-3cd3c8c20d64"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8239de6b-4298-4a32-8137-6597241c42fb"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1839bf59-f0b2-4a9e-8e5b-1fd4de95448b"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ff109d16-c838-4273-ae9b-d8030818f08e"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": ""NormalizeVector2"",
                    ""groups"": ""Controller"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53fe438a-fd28-4bdf-bf80-500e8b61011e"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone,NormalizeVector2"",
                    ""groups"": ""Controller"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24d90176-250a-4ac7-acf3-9faf72303334"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""189174e8-eb23-454a-aad2-0e2d9e2aa479"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9f3a8c4c-608f-4c3a-9c6c-b64772bdc132"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Sprint / Skip Dialogue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6d2c017a-3065-4222-a26d-9b7a537eb4b0"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Sprint / Skip Dialogue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""588e557d-4ba7-45de-b007-32fa9332c04c"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Open Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a01d2d12-d15b-4d10-9f62-e38c0b87d15c"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Open Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""04980a88-9533-4a44-8862-4133a7683b6b"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Repel Skill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e2c9e9b-bcf2-4f91-aa1e-5b967ad7553d"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Repel Skill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c17de111-2007-4f25-b509-d2f751db327d"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Taunt Skill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e8f66ebd-94cf-4953-b6dc-bc7af5e25ffb"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Taunt Skill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""MenuNavigation"",
            ""id"": ""afbe5985-1e0a-4d9c-8506-06155e898197"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""6e5f20f2-b05f-434d-8b57-336296f5c871"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Confirm"",
                    ""type"": ""Button"",
                    ""id"": ""76e1e754-04a0-423f-81f6-b61b4c337f00"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""166302c7-4b03-4b3c-8ee1-3b80c3acf4a6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Close Menu"",
                    ""type"": ""Button"",
                    ""id"": ""59698baa-f7de-4227-933d-08bfc61ecfc3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""f32c695b-2990-4584-a05d-5ea598184738"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6c306756-80f9-4956-ac70-e869a0f28aa1"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9c69f2d9-7620-49ea-bff6-6459204d6443"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d5b8cf31-7d2c-4534-98a4-862070ec3bc6"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""7323c530-8443-4ba6-9362-c7b9b6cd6a12"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow Keys"",
                    ""id"": ""6adc46b0-163d-4049-bc95-a583e9928baa"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""fd339321-d01a-47f6-ac6e-1c1abe2fdee4"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""19df0497-0e36-4972-b193-345f7c0fa679"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""aefd9740-3098-441d-995d-13012c9455bf"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""359393ec-02e2-4e27-b8e1-913c491fbabf"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""caa7f8d7-c0c4-4828-9555-3f22494586fd"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": ""NormalizeVector2"",
                    ""groups"": ""Controller"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b65a31e2-56c0-4689-bb15-f46d8242ea0b"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone,NormalizeVector2"",
                    ""groups"": ""Controller"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""016e377f-2427-44bf-b8b7-590339508b4e"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Close Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""73bc048e-47e8-45e0-a908-bad17d970269"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Close Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b800ccb0-8cb7-4e28-91c7-2419b224e5cb"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9db5d3a8-0c39-4c4c-9d24-3717ed484093"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5a6ee3b0-0b2e-4531-a1a8-ed6820180599"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1068e582-1a9a-4fc2-9944-f06d87d0b8ec"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Battle"",
            ""id"": ""c3561191-b88c-4dfe-9c80-0f75555bc815"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""1104561a-0e31-421a-bec5-542eb10e0e4a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Confirm"",
                    ""type"": ""Button"",
                    ""id"": ""8ca65a4b-eede-4fa0-8e92-0a66a7a3fd59"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""10a84d01-ef1d-4213-af89-b3bb77b742d6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DebugTrigger"",
                    ""type"": ""Button"",
                    ""id"": ""4f1242c4-038c-492d-9cd0-b4d6d7c6220a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""0d3cdfe0-49eb-44f0-bab8-a32140aadc22"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""31db9678-483b-4ef5-931a-717c1643d5c4"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""94b10721-ba07-493d-9857-7e424695d923"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d6c2c7a8-db79-452c-8e6f-452925cf5918"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9a04a8ee-eb8b-4022-959f-329253aa90df"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow Keys"",
                    ""id"": ""18e63b28-e7ff-4463-82f5-eb38a352cb8d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""eae4954f-a45f-44f3-98f0-ddecbfe63a04"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""5a89806d-9405-4b6e-8f4c-d84cdecda14b"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""33420ce6-ecbd-4662-b411-e5f9feff5289"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""bab58c85-a294-46b6-bb19-d1bd48063f30"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""af67944e-577d-4e63-829b-05a782011e55"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": ""NormalizeVector2"",
                    ""groups"": ""Controller"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1385048c-5ab3-4142-8e91-15a665b0d5a6"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone,NormalizeVector2"",
                    ""groups"": ""Controller"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d8ca60ac-8782-42a5-8d40-c0fa9aa59c2b"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a2e17cf4-21e7-4fc7-85da-8a5a9d37f8d3"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""585ef6bb-8243-444b-bd5a-624e7bb252ac"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""37a312ef-4553-489e-a063-4db4c5e5f9a1"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8216a43d-639b-4a65-9709-7fb247ab1735"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""DebugTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2012785d-5462-4c58-97bf-dc479c9cb7a5"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""DebugTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Overworld
            m_Overworld = asset.FindActionMap("Overworld", throwIfNotFound: true);
            m_Overworld_Movement = m_Overworld.FindAction("Movement", throwIfNotFound: true);
            m_Overworld_Interact = m_Overworld.FindAction("Interact", throwIfNotFound: true);
            m_Overworld_SprintSkipDialogue = m_Overworld.FindAction("Sprint / Skip Dialogue", throwIfNotFound: true);
            m_Overworld_OpenMenu = m_Overworld.FindAction("Open Menu", throwIfNotFound: true);
            m_Overworld_RepelSkill = m_Overworld.FindAction("Repel Skill", throwIfNotFound: true);
            m_Overworld_TauntSkill = m_Overworld.FindAction("Taunt Skill", throwIfNotFound: true);
            // MenuNavigation
            m_MenuNavigation = asset.FindActionMap("MenuNavigation", throwIfNotFound: true);
            m_MenuNavigation_Movement = m_MenuNavigation.FindAction("Movement", throwIfNotFound: true);
            m_MenuNavigation_Confirm = m_MenuNavigation.FindAction("Confirm", throwIfNotFound: true);
            m_MenuNavigation_Cancel = m_MenuNavigation.FindAction("Cancel", throwIfNotFound: true);
            m_MenuNavigation_CloseMenu = m_MenuNavigation.FindAction("Close Menu", throwIfNotFound: true);
            // Battle
            m_Battle = asset.FindActionMap("Battle", throwIfNotFound: true);
            m_Battle_Movement = m_Battle.FindAction("Movement", throwIfNotFound: true);
            m_Battle_Confirm = m_Battle.FindAction("Confirm", throwIfNotFound: true);
            m_Battle_Cancel = m_Battle.FindAction("Cancel", throwIfNotFound: true);
            m_Battle_DebugTrigger = m_Battle.FindAction("DebugTrigger", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // Overworld
        private readonly InputActionMap m_Overworld;
        private IOverworldActions m_OverworldActionsCallbackInterface;
        private readonly InputAction m_Overworld_Movement;
        private readonly InputAction m_Overworld_Interact;
        private readonly InputAction m_Overworld_SprintSkipDialogue;
        private readonly InputAction m_Overworld_OpenMenu;
        private readonly InputAction m_Overworld_RepelSkill;
        private readonly InputAction m_Overworld_TauntSkill;
        public struct OverworldActions
        {
            private @Inputs m_Wrapper;
            public OverworldActions(@Inputs wrapper) { m_Wrapper = wrapper; }
            public InputAction @Movement => m_Wrapper.m_Overworld_Movement;
            public InputAction @Interact => m_Wrapper.m_Overworld_Interact;
            public InputAction @SprintSkipDialogue => m_Wrapper.m_Overworld_SprintSkipDialogue;
            public InputAction @OpenMenu => m_Wrapper.m_Overworld_OpenMenu;
            public InputAction @RepelSkill => m_Wrapper.m_Overworld_RepelSkill;
            public InputAction @TauntSkill => m_Wrapper.m_Overworld_TauntSkill;
            public InputActionMap Get() { return m_Wrapper.m_Overworld; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(OverworldActions set) { return set.Get(); }
            public void SetCallbacks(IOverworldActions instance)
            {
                if (m_Wrapper.m_OverworldActionsCallbackInterface != null)
                {
                    @Movement.started -= m_Wrapper.m_OverworldActionsCallbackInterface.OnMovement;
                    @Movement.performed -= m_Wrapper.m_OverworldActionsCallbackInterface.OnMovement;
                    @Movement.canceled -= m_Wrapper.m_OverworldActionsCallbackInterface.OnMovement;
                    @Interact.started -= m_Wrapper.m_OverworldActionsCallbackInterface.OnInteract;
                    @Interact.performed -= m_Wrapper.m_OverworldActionsCallbackInterface.OnInteract;
                    @Interact.canceled -= m_Wrapper.m_OverworldActionsCallbackInterface.OnInteract;
                    @SprintSkipDialogue.started -= m_Wrapper.m_OverworldActionsCallbackInterface.OnSprintSkipDialogue;
                    @SprintSkipDialogue.performed -= m_Wrapper.m_OverworldActionsCallbackInterface.OnSprintSkipDialogue;
                    @SprintSkipDialogue.canceled -= m_Wrapper.m_OverworldActionsCallbackInterface.OnSprintSkipDialogue;
                    @OpenMenu.started -= m_Wrapper.m_OverworldActionsCallbackInterface.OnOpenMenu;
                    @OpenMenu.performed -= m_Wrapper.m_OverworldActionsCallbackInterface.OnOpenMenu;
                    @OpenMenu.canceled -= m_Wrapper.m_OverworldActionsCallbackInterface.OnOpenMenu;
                    @RepelSkill.started -= m_Wrapper.m_OverworldActionsCallbackInterface.OnRepelSkill;
                    @RepelSkill.performed -= m_Wrapper.m_OverworldActionsCallbackInterface.OnRepelSkill;
                    @RepelSkill.canceled -= m_Wrapper.m_OverworldActionsCallbackInterface.OnRepelSkill;
                    @TauntSkill.started -= m_Wrapper.m_OverworldActionsCallbackInterface.OnTauntSkill;
                    @TauntSkill.performed -= m_Wrapper.m_OverworldActionsCallbackInterface.OnTauntSkill;
                    @TauntSkill.canceled -= m_Wrapper.m_OverworldActionsCallbackInterface.OnTauntSkill;
                }
                m_Wrapper.m_OverworldActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Movement.started += instance.OnMovement;
                    @Movement.performed += instance.OnMovement;
                    @Movement.canceled += instance.OnMovement;
                    @Interact.started += instance.OnInteract;
                    @Interact.performed += instance.OnInteract;
                    @Interact.canceled += instance.OnInteract;
                    @SprintSkipDialogue.started += instance.OnSprintSkipDialogue;
                    @SprintSkipDialogue.performed += instance.OnSprintSkipDialogue;
                    @SprintSkipDialogue.canceled += instance.OnSprintSkipDialogue;
                    @OpenMenu.started += instance.OnOpenMenu;
                    @OpenMenu.performed += instance.OnOpenMenu;
                    @OpenMenu.canceled += instance.OnOpenMenu;
                    @RepelSkill.started += instance.OnRepelSkill;
                    @RepelSkill.performed += instance.OnRepelSkill;
                    @RepelSkill.canceled += instance.OnRepelSkill;
                    @TauntSkill.started += instance.OnTauntSkill;
                    @TauntSkill.performed += instance.OnTauntSkill;
                    @TauntSkill.canceled += instance.OnTauntSkill;
                }
            }
        }
        public OverworldActions @Overworld => new OverworldActions(this);

        // MenuNavigation
        private readonly InputActionMap m_MenuNavigation;
        private IMenuNavigationActions m_MenuNavigationActionsCallbackInterface;
        private readonly InputAction m_MenuNavigation_Movement;
        private readonly InputAction m_MenuNavigation_Confirm;
        private readonly InputAction m_MenuNavigation_Cancel;
        private readonly InputAction m_MenuNavigation_CloseMenu;
        public struct MenuNavigationActions
        {
            private @Inputs m_Wrapper;
            public MenuNavigationActions(@Inputs wrapper) { m_Wrapper = wrapper; }
            public InputAction @Movement => m_Wrapper.m_MenuNavigation_Movement;
            public InputAction @Confirm => m_Wrapper.m_MenuNavigation_Confirm;
            public InputAction @Cancel => m_Wrapper.m_MenuNavigation_Cancel;
            public InputAction @CloseMenu => m_Wrapper.m_MenuNavigation_CloseMenu;
            public InputActionMap Get() { return m_Wrapper.m_MenuNavigation; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MenuNavigationActions set) { return set.Get(); }
            public void SetCallbacks(IMenuNavigationActions instance)
            {
                if (m_Wrapper.m_MenuNavigationActionsCallbackInterface != null)
                {
                    @Movement.started -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnMovement;
                    @Movement.performed -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnMovement;
                    @Movement.canceled -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnMovement;
                    @Confirm.started -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnConfirm;
                    @Confirm.performed -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnConfirm;
                    @Confirm.canceled -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnConfirm;
                    @Cancel.started -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnCancel;
                    @Cancel.performed -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnCancel;
                    @Cancel.canceled -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnCancel;
                    @CloseMenu.started -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnCloseMenu;
                    @CloseMenu.performed -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnCloseMenu;
                    @CloseMenu.canceled -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnCloseMenu;
                }
                m_Wrapper.m_MenuNavigationActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Movement.started += instance.OnMovement;
                    @Movement.performed += instance.OnMovement;
                    @Movement.canceled += instance.OnMovement;
                    @Confirm.started += instance.OnConfirm;
                    @Confirm.performed += instance.OnConfirm;
                    @Confirm.canceled += instance.OnConfirm;
                    @Cancel.started += instance.OnCancel;
                    @Cancel.performed += instance.OnCancel;
                    @Cancel.canceled += instance.OnCancel;
                    @CloseMenu.started += instance.OnCloseMenu;
                    @CloseMenu.performed += instance.OnCloseMenu;
                    @CloseMenu.canceled += instance.OnCloseMenu;
                }
            }
        }
        public MenuNavigationActions @MenuNavigation => new MenuNavigationActions(this);

        // Battle
        private readonly InputActionMap m_Battle;
        private IBattleActions m_BattleActionsCallbackInterface;
        private readonly InputAction m_Battle_Movement;
        private readonly InputAction m_Battle_Confirm;
        private readonly InputAction m_Battle_Cancel;
        private readonly InputAction m_Battle_DebugTrigger;
        public struct BattleActions
        {
            private @Inputs m_Wrapper;
            public BattleActions(@Inputs wrapper) { m_Wrapper = wrapper; }
            public InputAction @Movement => m_Wrapper.m_Battle_Movement;
            public InputAction @Confirm => m_Wrapper.m_Battle_Confirm;
            public InputAction @Cancel => m_Wrapper.m_Battle_Cancel;
            public InputAction @DebugTrigger => m_Wrapper.m_Battle_DebugTrigger;
            public InputActionMap Get() { return m_Wrapper.m_Battle; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(BattleActions set) { return set.Get(); }
            public void SetCallbacks(IBattleActions instance)
            {
                if (m_Wrapper.m_BattleActionsCallbackInterface != null)
                {
                    @Movement.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnMovement;
                    @Movement.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnMovement;
                    @Movement.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnMovement;
                    @Confirm.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnConfirm;
                    @Confirm.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnConfirm;
                    @Confirm.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnConfirm;
                    @Cancel.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnCancel;
                    @Cancel.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnCancel;
                    @Cancel.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnCancel;
                    @DebugTrigger.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnDebugTrigger;
                    @DebugTrigger.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnDebugTrigger;
                    @DebugTrigger.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnDebugTrigger;
                }
                m_Wrapper.m_BattleActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Movement.started += instance.OnMovement;
                    @Movement.performed += instance.OnMovement;
                    @Movement.canceled += instance.OnMovement;
                    @Confirm.started += instance.OnConfirm;
                    @Confirm.performed += instance.OnConfirm;
                    @Confirm.canceled += instance.OnConfirm;
                    @Cancel.started += instance.OnCancel;
                    @Cancel.performed += instance.OnCancel;
                    @Cancel.canceled += instance.OnCancel;
                    @DebugTrigger.started += instance.OnDebugTrigger;
                    @DebugTrigger.performed += instance.OnDebugTrigger;
                    @DebugTrigger.canceled += instance.OnDebugTrigger;
                }
            }
        }
        public BattleActions @Battle => new BattleActions(this);
        private int m_ControllerSchemeIndex = -1;
        public InputControlScheme ControllerScheme
        {
            get
            {
                if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
                return asset.controlSchemes[m_ControllerSchemeIndex];
            }
        }
        private int m_KeyboardSchemeIndex = -1;
        public InputControlScheme KeyboardScheme
        {
            get
            {
                if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
                return asset.controlSchemes[m_KeyboardSchemeIndex];
            }
        }
        public interface IOverworldActions
        {
            void OnMovement(InputAction.CallbackContext context);
            void OnInteract(InputAction.CallbackContext context);
            void OnSprintSkipDialogue(InputAction.CallbackContext context);
            void OnOpenMenu(InputAction.CallbackContext context);
            void OnRepelSkill(InputAction.CallbackContext context);
            void OnTauntSkill(InputAction.CallbackContext context);
        }
        public interface IMenuNavigationActions
        {
            void OnMovement(InputAction.CallbackContext context);
            void OnConfirm(InputAction.CallbackContext context);
            void OnCancel(InputAction.CallbackContext context);
            void OnCloseMenu(InputAction.CallbackContext context);
        }
        public interface IBattleActions
        {
            void OnMovement(InputAction.CallbackContext context);
            void OnConfirm(InputAction.CallbackContext context);
            void OnCancel(InputAction.CallbackContext context);
            void OnDebugTrigger(InputAction.CallbackContext context);
        }
    }
}
