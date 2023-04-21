
import { Link } from 'react-router-dom';
import { useQuery } from '../utils/utils';
const NotFoundAdmin = () => {
  let query = useQuery(),
    redirectTo = query.get('redirectTo') ?? '/';
  return <> … </>;
};
export default NotFoundAdmin;
