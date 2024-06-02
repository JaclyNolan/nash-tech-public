import {
    Autocomplete,
    AutocompleteRenderInputParams,
    Box,
    CircularProgress,
    TextField
} from '@mui/material';
import debounce from 'lodash/debounce';
import React, { useEffect, useMemo, useRef, useState } from 'react';

export interface SelectOption {
    value: number | string;
    label: string;
}

interface DebounceSelectProps {
    fetchOptions: (input: string) => Promise<SelectOption[]>;
    debounceTimeout?: number;
    presetOptions?: SelectOption[];
    name: string;
    label: string;
    value?: SelectOption | null;
    onChange?: (value: SelectOption | null) => void;
    [key: string]: any; // Allow any other props
}

const DebounceSelect: React.FC<DebounceSelectProps> = ({
    fetchOptions,
    debounceTimeout = 800,
    presetOptions = [],
    name,
    label,
    value = null,
    onChange,
    ...props
}) => {
    const [open, setOpen] = useState(false);
    const [fetching, setFetching] = useState(false);
    const [options, _setOptions] = useState<SelectOption[]>(presetOptions);
    const [autoValue, setAutoValue] = useState<SelectOption | null>(value);
    const [searchInput, setSearchInput] = useState('');

    const setOptions = (newOptions: SelectOption[] = []) => {
        presetOptions.forEach((presetOption) => {
            const matchingIndex = newOptions.findIndex((newOption) => presetOption.value === newOption.value);
            if (matchingIndex !== -1) newOptions.splice(matchingIndex, 1);
        });
        if (presetOptions) {
            _setOptions([...presetOptions, ...newOptions]);
        } else {
            _setOptions(newOptions);
        }
    };

    const fetchRef = useRef(0);

    const debounceFetcher = useMemo(() => {
        const loadOptions = (value: string) => {
            fetchRef.current += 1;
            const fetchId = fetchRef.current;
            setOptions([]);
            setFetching(true);
            fetchOptions(value).then((newOptions) => {
                if (fetchId !== fetchRef.current) {
                    return;
                }
                setOptions(newOptions);
                setFetching(false);
            });
        };
        return debounce(loadOptions, debounceTimeout);
    }, [fetchOptions, debounceTimeout]);

    useEffect(() => {
        debounceFetcher(searchInput);
    }, [searchInput, debounceFetcher]);

    return (
        <Autocomplete
            filterOptions={(x) => x} // Disable built-in filter/search
            fullWidth
            open={open}
            id='debounce-select'
            onOpen={() => setOpen(true)}
            onClose={() => setOpen(false)}
            isOptionEqualToValue={(option, value) => option.value === value.value}
            getOptionLabel={(option) => `${option.label}`}
            value={autoValue}
            onChange={(_, newValue) => {
                setAutoValue(newValue);
                setSearchInput('');
                if (onChange) onChange(newValue);
            }}
            options={options}
            loading={fetching}
            onInputChange={(_, value) => {
                if (value !== autoValue?.value) {
                    setSearchInput(value);
                }
            }}
            renderOption={(props, option) => (
                <Box component="li" sx={{ '& > img': { mr: 2, flexShrink: 0 } }} {...props}>
                    {option.label}
                </Box>
            )}
            renderInput={(params: AutocompleteRenderInputParams) => (
                <TextField
                    {...params}
                    {...props}
                    label={label}
                    InputProps={{
                        ...params.InputProps,
                        endAdornment: (
                            <>
                                {fetching ? <CircularProgress color="inherit" size={20} /> : null}
                                {params.InputProps.endAdornment}
                            </>
                        ),
                    }}
                />
            )}
        />
    );
};

export default DebounceSelect;
