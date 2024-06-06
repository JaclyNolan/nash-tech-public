import { LoadingButton } from '@mui/lab';
import { Grid, Stack } from '@mui/material';
import TextField from '@mui/material/TextField';
import Typography from '@mui/material/Typography';
import { useFormik } from 'formik';
import { FC, useState } from 'react';
import * as yup from 'yup';
import axiosInstance from '../../../axiosInstance';
import { URLConstants } from '../../../common/constants';
import { Category } from '../BookPages/BookAdd';

interface CategoryAddProps {
  fetchList: () => void;
}

// Validation schema
const validationSchema = yup.object({
  name: yup.string().required('Name is required').max(200, 'Name length can\'t be more than 200 characters.'),
});

const CategoryAdd: FC<CategoryAddProps> = ({ fetchList }) => {
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
        const response = await axiosInstance.post(URLConstants.CATEGORY.ADD, payload);
        const category: Category = response.data;
        alert(`Added successfully! Id: ${category.id}, Name: ${category.name}`);
        fetchList();
      } catch (error) {
        console.error('Error adding category:', error);
      } finally {
        setIsSubmitting(false);
      }
    },
  });

  return (
    <Stack spacing={3}>
      <Typography variant="h6" gutterBottom>
        Add Category
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
              Add
            </LoadingButton>
          </Grid>
        </Grid>
      </form>
    </Stack>
  );
}

export default CategoryAdd;
