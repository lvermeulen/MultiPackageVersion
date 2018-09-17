using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MultiPackageVersion.Config.Tests
{
    public class ConfigurationShould
    {
        [Fact]
        public void Save()
        {
            var config = new Configuration
            {
                DefaultVersionIncrement = VersionIncrementType.Patch,
                Entries = new Dictionary<string, VersionConfiguration>
                {
                    ["Libraries\\*"] = new VersionConfiguration
                    {
                        Version = "1.2.3",
                        VersionIncrement = VersionIncrementType.Major,
                        VersionConfigurationEntries = new List<VersionConfigurationEntry>
                        {
                            new VersionConfigurationEntry
                            {
                                NuspecFileName = "Saga.ApiErrors.nuspec",
                                SolutionFileName = "Saga.ApiErrors.sln",
                                BuildDefinitionName = "Saga.ApiErrors.config.xml"
                            }
                        }
                    },
                    ["Workflow\\Workflow.Domain.*"] = new VersionConfiguration
                    {
                        Version = "4.5.6",
                        VersionIncrement = VersionIncrementType.Minor,
                        VersionConfigurationEntries = new List<VersionConfigurationEntry>
                        {
                            new VersionConfigurationEntry
                            {
                                NuspecFileName = "Workflow.Domain.Models.nuspec",
                                SolutionFileName = "Workflow.Domain.Models.sln",
                                BuildDefinitionName = "Workflow.Domain.Models.config.xml"
                            },
                            new VersionConfigurationEntry
                            {
                                NuspecFileName = "Workflow.Domain.Workflow.nuspec",
                                SolutionFileName = "Workflow.Domain.Workflow.sln",
                                BuildDefinitionName = "Workflow.Domain.Workflow.config.xml"
                            }
                        }
                    },
                    ["Workflow\\Workflow.Application.*"] = new VersionConfiguration
                    {
                        Version = "5.6.7",
                        VersionIncrement = VersionIncrementType.Patch,
                        VersionConfigurationEntries = new List<VersionConfigurationEntry>
                        {
                            new VersionConfigurationEntry
                            {
                                NuspecFileName = "Workflow.Application.WorkflowDesigns.nuspec",
                                SolutionFileName = "Workflow.Application.WorkflowDesigns.sln",
                                BuildDefinitionName = "Workflow.Application.WorkflowDesigns.config.xml"
                            },
                            new VersionConfigurationEntry
                            {
                                NuspecFileName = "Workflow.Application.WorkForce.nuspec",
                                SolutionFileName = "Workflow.Application.WorkForce.sln",
                                BuildDefinitionName = "Workflow.Application.WorkForce.config.xml"
                            }
                        }
                    }
                }
            };

            Configuration.Save(config, "config.json");
        }

        [Fact]
        public void Parse()
        {
            #region JSON

            const string JSON = @"{
  ""DefaultVersionIncrementType"": ""patch"",
  ""Entries"": {
    ""Libraries\\*"": {
      ""Version"": ""1.2.3"",
      ""VersionIncrement"": ""major"",
      ""VersionConfigurationEntries"": [
        {
          ""NuspecFileName"": ""Saga.ApiErrors.nuspec"",
          ""SolutionFileName"": ""Saga.ApiErrors.sln"",
          ""BuildDefinitionName"": ""Saga.ApiErrors.config.xml""
        }
      ]
    },
    ""Workflow\\Workflow.Domain.*"": {
      ""Version"": ""4.5.6"",
      ""VersionIncrement"": ""minor"",
      ""VersionConfigurationEntries"": [
        {
          ""NuspecFileName"": ""Workflow.Domain.Models.nuspec"",
          ""SolutionFileName"": ""Workflow.Domain.Models.sln"",
          ""BuildDefinitionName"": ""Workflow.Domain.Models.config.xml""
        },
        {
          ""NuspecFileName"": ""Workflow.Domain.Workflow.nuspec"",
          ""SolutionFileName"": ""Workflow.Domain.Workflow.sln"",
          ""BuildDefinitionName"": ""Workflow.Domain.Workflow.config.xml""
        }
      ]
    },
    ""Workflow\\Workflow.Application.*"": {
      ""Version"": ""5.6.7"",
      ""VersionIncrement"": ""patch"",
      ""VersionConfigurationEntries"": [
        {
          ""NuspecFileName"": ""Workflow.Application.WorkflowDesigns.nuspec"",
          ""SolutionFileName"": ""Workflow.Application.WorkflowDesigns.sln"",
          ""BuildDefinitionName"": ""Workflow.Application.WorkflowDesigns.config.xml""
        },
        {
          ""NuspecFileName"": ""Workflow.Application.WorkForce.nuspec"",
          ""SolutionFileName"": ""Workflow.Application.WorkForce.sln"",
          ""BuildDefinitionName"": ""Workflow.Application.WorkForce.config.xml""
        }
      ]
    }
  }
}";

            #endregion

            var config = Configuration.Parse(JSON);

            Assert.Equal(3, config.Entries.Count);
            Assert.Equal(2, config.Entries.Last().Value.VersionConfigurationEntries.Count);
        }
    }
}
