﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// MIT License
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Collections.Generic;
using Hl7.Fhir.Model;
using Microsoft.HealthVault.Fhir.Constants;
using Microsoft.HealthVault.ItemTypes;

namespace Microsoft.HealthVault.Fhir.Transformers
{
    public static partial class ThingBaseToFhir
    {
        // Register the type on the generic ThingToFhir partial class
        public static Patient ToFhir(this Personal personal)
        {
            return PersonalToFhir.ToFhirInternal(personal, ThingBaseToFhir.ToFhirInternal<Patient>(personal));
        }

        // Register the type on the generic ThingToFhir partial class
        public static Patient ToFhir(this Personal personal, Patient patient)
        {
            return PersonalToFhir.ToFhirInternal(personal, patient);
        }
    }

    /// <summary>
    /// An extension class that transforms HealthVault personal data types into FHIR Patient
    /// </summary>
    internal static class PersonalToFhir
    {
        internal static Patient ToFhirInternal(Personal personal, Patient patient)
        {
            if (personal.BirthDate?.Date != null)
            {
                patient.BirthDateElement = new Date(personal.BirthDate.Date.Year, personal.BirthDate.Date.Month, personal.BirthDate.Date.Day);

                if (personal.BirthDate?.Time != null)
                {
                    patient.BirthDateElement.Extension.Add(new Extension(HealthVaultExtensions.PatientBirthTime,personal.BirthDate.Time.ToFhir()));
                }
            }

            if (personal.DateOfDeath != null)
            {
                patient.Deceased = personal.DateOfDeath.ToFhir();
            }
            else
            {
                patient.Deceased = new FhirBoolean(personal.IsDeceased);
            }

            if (personal.BloodType != null)
            {
                patient.Extension.Add(new Extension(HealthVaultExtensions.PatientBloodType, personal.BloodType.ToFhir()));
            }

            if (!string.IsNullOrEmpty(personal.EmploymentStatus))
            {
                patient.Extension.Add(new Extension(HealthVaultExtensions.PatientEmploymentStatus,new FhirString(personal.EmploymentStatus)));
            }

            if (personal.Ethnicity != null)
            {
                patient.Extension.Add(new Extension(HealthVaultExtensions.PatientEthnicity, personal.Ethnicity.ToFhir()));
            }

            if (personal.HighestEducationLevel != null)
            {
                patient.Extension.Add(new Extension(HealthVaultExtensions.PatientHighestEducationLevel,personal.HighestEducationLevel.ToFhir()));
            }

            if (personal.IsDisabled.HasValue)
            {
                patient.Extension.Add(new Extension(HealthVaultExtensions.PatientIsDisabled, new FhirBoolean(personal.IsDisabled)));
            }

            if (personal.IsVeteran.HasValue)
            {
                patient.Extension.Add(new Extension(HealthVaultExtensions.PatientIsVeteran, new FhirBoolean(personal.IsVeteran)));
            }

            if (personal.MaritalStatus != null)
            {
                patient.Extension.Add(new Extension(HealthVaultExtensions.PatientMaritalStatus, personal.MaritalStatus.ToFhir()));
            }

            if (personal.Name != null)
            {
                var humanName = new HumanName
                {
                    Family = personal.Name.Last,
                    Text = personal.Name.Full,
                };

                var givenNames = new List<string>();
                if (!string.IsNullOrEmpty(personal.Name.First))
                {
                    givenNames.Add(personal.Name.First);
                }

                if (!string.IsNullOrEmpty(personal.Name.Middle))
                {
                    givenNames.Add(personal.Name.Middle);
                }
                humanName.Given = givenNames;

                if (personal.Name.Title != null)
                {
                    humanName.Extension.Add(new Extension(HealthVaultExtensions.PatientTitle,personal.Name.Title.ToFhir()));
                }

                if (personal.Name.Suffix != null)
                {
                    humanName.Extension.Add(new Extension(HealthVaultExtensions.PatientSuffix, personal.Name.Suffix.ToFhir()));
                }

                patient.Name.Add(humanName);
            }

            if (!string.IsNullOrEmpty(personal.OrganDonor))
            {
                patient.Extension.Add(new Extension(HealthVaultExtensions.PatientOrganDonor, new FhirString(personal.OrganDonor)));
            }

            if (personal.Religion != null)
            {
                patient.Extension.Add(new Extension(HealthVaultExtensions.PatientReligion, personal.Religion.ToFhir()));
            }

            if (!string.IsNullOrEmpty(personal.SocialSecurityNumber))
            {
                patient.Identifier.Add(new Identifier{
                        Value = personal.SocialSecurityNumber,
                        System = Constants.FhirExtensions.SSN,
                    }
                );
            }
            return patient;
        }
    }
}
