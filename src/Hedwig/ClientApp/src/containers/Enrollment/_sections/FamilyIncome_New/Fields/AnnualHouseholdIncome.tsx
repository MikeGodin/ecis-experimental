import FormField from "../../../../../components/Form_New/FormField"
import { Enrollment, FamilyDetermination } from "../../../../../generated"
import React, { ChangeEvent } from "react"
import { TextInputProps, TextInput } from "../../../../../components"
import parseCurrencyFromString from "../../../../../utils/parseCurrencyFromString"
import currencyFormatter from "../../../../../utils/currencyFormatter"
import { warningForField } from "../../../../../utils/validations"

export const AnnualHouseholdIncomeField = ({ id }: { id: number}) => {
	return <FormField<Enrollment, TextInputProps, number | null>
		getValue={data => 
			data
				.at('child')
				.at('family')
				.at('determinations')
				.find((det:FamilyDetermination) => det.id === id)
				.at('income')
		}
		parseOnChangeEvent={(e: ChangeEvent<HTMLInputElement>) => parseCurrencyFromString(e.target.value)}
		preprocessForDisplay={income => currencyFormatter(income)}
		inputComponent={TextInput}
		props={{
			id: `income-${id}`,
			label: 'Annual household income',
			onBlur: (event: ChangeEvent<HTMLInputElement>) => { event.target.value = currencyFormatter(parseInt(event.target.value))}
		}}
		status={(data) => 
			warningForField(
				'income', 
				data.at('child').at('family').at('determinations').find((det: FamilyDetermination) => det.id === id).value,
				''
			)
		}
	/>
}
