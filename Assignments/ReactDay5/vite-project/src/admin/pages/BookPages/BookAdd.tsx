import { LoadingButton } from '@mui/lab';
import { Grid, Stack } from '@mui/material';
import TextField from '@mui/material/TextField';
import Typography from '@mui/material/Typography';
import { useState, useCallback, FC } from 'react';
import { useFormik } from 'formik';
import * as yup from 'yup';
import axiosInstance from '../../../axiosInstance';
import { URLConstants } from '../../../common/constants';
import DebounceSelect, { SelectOption } from '../../../shared/components/table/DebounceSelect';
import { FetchData } from './BookList';
import { AxiosResponse } from 'axios';

interface BookAddProps {
  fetchList: () => void;
}

export interface Category {
  id: string;
  name: string;
  dateCreated: string;
}


// Validation schema
const validationSchema = yup.object({
  title: yup.string().required('Title is required').max(200, 'Title length can\'t be more than 200 characters.'),
  author: yup.string().max(200, 'Author length can\'t be more than 200 characters.'),
  description: yup.string().max(500, 'Description length can\'t be more than 500 characters.'),
  category: yup.object().nullable().required('Category is required'),
});

const BookAdd: FC<BookAddProps> = ({ fetchList }) => {
  const [isSubmitting, setIsSubmitting] = useState(false);

  const fetchCategoryData = useCallback(async (search: string): Promise<SelectOption[]> => {
    const params = { search };
    const response: AxiosResponse<FetchData<Category>> = await axiosInstance.get(URLConstants.CATEGORY.GETALL, { params });
    const categories: Category[] = response.data.data;
    return categories.map((category) => ({
      value: category.id,
      label: `${category.id} ${category.name}`,
    }));
  }, []);

  const formik = useFormik({
    initialValues: {
      title: '',
      author: '',
      description: '',
      category: null as SelectOption | null,
    },
    validationSchema: validationSchema,
    onSubmit: async (values) => {
      setIsSubmitting(true);
      const payload = {
        title: values.title,
        author: values.author,
        description: values.description,
        category_id: values.category?.value,
      };
      try {
        const response = await axiosInstance.post(URLConstants.BOOK.ADD, payload);
        const book = response.data;
        alert(`Added successfully! Id: ${book.id}, Title: ${book.title}`);
        fetchList();
      } catch (error) {
        console.error('Error adding book:', error);
      } finally {
        setIsSubmitting(false);
      }
    },
  });

  return (
    <Stack spacing={3}>
      <Typography variant="h6" gutterBottom>
        Add Book
      </Typography>
      <form onSubmit={formik.handleSubmit}>
        <Grid container spacing={3}>
          <Grid item xs={12} sm={6}>
            <TextField
              required
              fullWidth
              id="title"
              name="title"
              label="Book Title"
              value={formik.values.title}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              error={formik.touched.title && Boolean(formik.errors.title)}
              helperText={formik.touched.title && formik.errors.title}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              id="author"
              name="author"
              label="Author"
              value={formik.values.author}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              error={formik.touched.author && Boolean(formik.errors.author)}
              helperText={formik.touched.author && formik.errors.author}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <DebounceSelect
              required
              id="category"
              name="category"
              label="Category"
              placeholder="Select Category"
              value={formik.values.category}
              onChange={(value) => formik.setFieldValue('category', value)}
              fetchOptions={fetchCategoryData}
              error={formik.touched.category && Boolean(formik.errors.category)}
              helperText={formik.touched.category && formik.errors.category}
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              id="description"
              name="description"
              label="Description"
              value={formik.values.description}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              fullWidth
              multiline
              rows={4}
              placeholder="Description"
              error={formik.touched.description && Boolean(formik.errors.description)}
              helperText={formik.touched.description && formik.errors.description}
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

export default BookAdd