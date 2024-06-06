import { LoadingButton } from '@mui/lab';
import { Grid, Stack } from '@mui/material';
import TextField from '@mui/material/TextField';
import Typography from '@mui/material/Typography';
import { useState, useEffect, FC } from 'react';
import { useFormik } from 'formik';
import * as yup from 'yup';
import axiosInstance from '../../../axiosInstance';
import { URLConstants } from '../../../common/constants';
import { Category } from '../BookPages/BookAdd';
import { AxiosResponse } from 'axios';

interface CategoryEditProps {
    fetchList: () => void;
    entryId: string;
}

// Validation schema
const validationSchema = yup.object({
    name: yup.string().required('Name is required').max(200, 'Name length can\'t be more than 200 characters.'),
});

const CategoryEdit: FC<CategoryEditProps> = ({ fetchList, entryId }) => {
    const [isSubmitting, setIsSubmitting] = useState(false);

    const formik = useFormik({
        initialValues: {
            name: '',
        },
        validationSchema: validationSchema,
        onSubmit: async (values) => {
            setIsSubmitting(true);
            const payload = {
                name: values.name,
            };
            try {
                await axiosInstance.put(`${URLConstants.CATEGORY.GETID}/${entryId}`, payload);
                alert(`Updated successfully! Id: ${entryId}, Name: ${payload.name}`);
                fetchList();
            } catch (error) {
                console.error('Error updating category:', error);
            } finally {
                setIsSubmitting(false);
            }
        },
    });

    useEffect(() => {
        const fetchCategoryDetails = async () => {
            try {
                const response: AxiosResponse = await axiosInstance.get(`${URLConstants.CATEGORY.GETID}/${entryId}`);
                const category: Category = response.data;
                
                formik.setValues({
                    name: category.name
                });
            } catch (error) {
                console.error('Error fetching category details:', error);
            }
        };

        fetchCategoryDetails();
    }, [entryId]);

    return (
        <Stack spacing={3}>
            <Typography variant="h6" gutterBottom>
                Edit Category
            </Typography>
            <form onSubmit={formik.handleSubmit}>
                <Grid container spacing={3}>
                    <Grid item xs={12} sm={6}>
                        <TextField
                            required
                            fullWidth
                            id="name"
                            name="name"
                            label="Category Name"
                            value={formik.values.name}
                            onChange={formik.handleChange}
                            onBlur={formik.handleBlur}
                            error={formik.touched.name && Boolean(formik.errors.name)}
                            helperText={formik.touched.name && formik.errors.name}
                        />
                    </Grid>
                    <Grid item xs={12} sm={6}>
                        <LoadingButton loading={isSubmitting} fullWidth size="large" type="submit" variant="contained">
                            Update
                        </LoadingButton>
                    </Grid>
                </Grid>
            </form>
        </Stack>
    );
}

export default CategoryEdit;
