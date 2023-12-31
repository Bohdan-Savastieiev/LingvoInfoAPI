﻿using LanguageStudyAPI.Models;
using LingvoInfoAPI.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace LingvoInfoAPI.Mappers
{
    public class LingvoWordFormsDtoMapper
    {
        public List<WordForm> MapWordForms(List<LingvoWordFormsDto> forms)
        {
            var result = new List<WordForm>();
            foreach (var form in forms)
            {
                var groups = form.ParadigmJson.Groups;
                for (int i = 0; i < groups.Length; i++)
                {
                    var table = groups[i].Table;
                    for (int j = 0; j < table.Length; j++)
                    {
                        for (int k = 0; k < table[j].Length; k++)
                        {
                            var value = table[j][k].Value;
                            if (!result.Select(x => x.Text).Contains(value) 
                                && ValidateWordFormValue(form.Lexem, value))
                            {
                                value = value.Replace("*", "");
                                if (value != "-" && value != "Plural" && value != "Singular")
                                {
                                    result.Add(new WordForm { Text = value.ToLowerInvariant() });
                                }
                            }
                        }
                    }
                }
            }

            return result
                .GroupBy(wf => wf.Text)
                .Select(group => group.First()) // Select one representative WordForm from each group
                .ToList(); ;
        }

        private bool ValidateWordFormValue(string lexem, string value)
        {
            if (value.IsNullOrEmpty()
                || !lexem.Contains(" ") && value.Contains(" ")
                || lexem.All(char.IsUpper) && !value.Contains(lexem)
                || char.IsLower(lexem.First()) && char.IsUpper(value.First()))
            {
                return false;
            }

            return true;
        }
    }
}
