import { BrowserRouter as Router , Routes , Route } from 'react-router-dom';
import '../src/index.css';
// import Navbar from './Components/NavBar'
// import Sidebar from './Components/Sidebar'
import Foodter from './Components/Foodter'
import Index from './pages/Index'
import Layout from './pages/Layout'
import Contact from './pages/Contact'
import About from './pages/About'
import Rss from './pages/Rss'
import NotFound from './pages/NotFound';

import AdminLayout from './pages/Admin/Layout'
import * as AdminIndex from './pages/Admin/Index'
import Authors from './pages/Admin/Authors'
import Categories from './pages/Admin/catagories'
import Comments from './pages/Admin/comments'
import Tags from './pages/Admin/tags'
import Post from './pages/Admin/post/Posts'
import * as PostEdit from './pages/Admin/post/Edit'
import BadRequest from './pages/Admin/BadRequest';

export default function App() {
  return (
     <Router>
            <Routes>
              <Route path='/' element={<Layout/>}>
                <Route path='/' element={<Index/>} />
                <Route path='blog' element={<Index/>} />
                <Route path='blog/Contact' element={<Contact/>} />
                <Route path='blog/About' element={<About/>} />
                <Route path='blog/RSS' element={<Rss/>} />
                </Route>
                <Route path= "*" element={<NotFound/>}/>


                <Route path='/admin' element={<AdminLayout/>}>
                    <Route path='/admin' element={<AdminIndex.default/>}></Route>
                    <Route path='/admin/authors' element={<Authors/>}></Route>
                    <Route path='/admin/categories' element={<Categories/>}></Route>
                    <Route path='/admin/comments' element={<Comments/>}></Route>
                    <Route path='/admin/tags' element={<Tags/>}></Route>
                    <Route path='/admin/posts' element={<Post/>}></Route>
                    <Route path="/admin/posts/edit" element={<PostEdit.default />} />
                </Route>
                <Route path= "/404" element={<BadRequest/>}/>
            </Routes>
            <Foodter/>
    </Router>

  );
}


