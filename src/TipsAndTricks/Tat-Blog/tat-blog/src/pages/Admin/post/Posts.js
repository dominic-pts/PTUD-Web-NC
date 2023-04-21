import React, { useEffect, useState } from 'react';
import Button from 'react-bootstrap/Button';
import Table from 'react-bootstrap/Table';
import { useSelector } from 'react-redux';
import { Link, useParams } from 'react-router-dom';
import Loading from '../../../Components/Loading';
import Pager from '../../../Components/Pager';
import { deletePostById, getPosts, switchPostPublished } from '../../../Services/BlogREpository';
import { useQuery } from '../../../Utils/Utils';
import PostFilterPane from './PostFIlterPane';
import VerifyModal from '../../../Components/VerifyModal';

const Posts = () => {
  const [postsList, setPostsList] = useState([]);
  const [metadata, setMetadata] = useState({});
  const [postQuery, setPostQuery] = useState({});
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [isVisibleLoading, setIsVisibleLoading] = useState(true);
  const [postDelete, setPostDelete] = useState({});
  const postFilter = useSelector((state) => state.postFilter);

  let { id } = useParams();
  const query = useQuery();
  const p = query.get('p') ?? 1;
  const ps = query.get('ps') ?? 2;

  const handleCloseDeleteModal = () => setShowDeleteModal(false);
  const handleShowDeleteModal = (post) => {
    setShowDeleteModal(true);
    setPostDelete(post);
  };

  useEffect(() => {
    document.title = 'Danh sách bài viết';
    getPosts(
      postFilter.keyword,
      ps,
      p,
      postFilter.authorId,
      postFilter.categoryId,
      postFilter.year,
      postFilter.month,
      postFilter.published
    ).then((data) => {
      if (data) {
        setPostQuery((pre) => {
          return { ...pre, to: '/admin/posts' };
        });
        setPostsList(data.items);
        setMetadata(data.metadata);
      } else setPostsList([]);
      setIsVisibleLoading(false);
    });
  }, [
    postFilter.keyword,
    postFilter.authorId,
    postFilter.categoryId,
    postFilter.year,
    postFilter.month,
    postFilter.published,
    p,
    ps,
  ]);

  const publishedOnChanged = (id) => {
    switchPostPublished(id).then((data) => {
      if (data) {
        setPostsList((pre) => {
          return pre.filter((post) => post.id !== id);
        });
      }
    });
  };

  const deletedChanged = (id) => {
    deletePostById(id).then((data) => {
      if (data) {
        setPostsList((pre) => {
          alert('Post deleted');
          return pre.filter((post) => post.id !== id);
        });
      }
    });
  };

  return (
    <>
      <h1>Danh sách bài viết {id}</h1>
      <PostFilterPane />
      {isVisibleLoading ? (
        <Loading />
      ) : (
        <Table striped responsive bordered>
          <thead>
            <tr>
              <th width="65%">Tiêu đề</th>
              <th width="10%">Tác giả</th>
              <th width="10%">Chủ đề</th>
              <th width="7.5%">Xuất bản</th>
              <th width="7.5%">Xoá bài</th>
            </tr>
          </thead>
          <tbody>
            {postsList && postsList.length > 0 ? (
              postsList.map((item, index) => (
                <tr key={index}>
                  <td>
                    <Link to={`/admin/posts/edit/${item.id}`} className="text-bold">
                      {item.title}
                    </Link>
                    <p className="text-muted">{item.shortDescription}</p>
                  </td>
                  <td>{item.author.fullName}</td>
                  <td>{item.category.name}</td>
                  <td>
                    {item.published ? (
                      <Button onClick={(e) => publishedOnChanged(item.id)} variant="success">
                        Có
                      </Button>
                    ) : (
                      <Button onClick={(e) => publishedOnChanged(item.id)} variant="warning">
                        Không
                      </Button>
                    )}
                  </td>
                  <td>
                    <Button onClick={(e) => handleShowDeleteModal(item)} variant="danger">
                      Xoá
                    </Button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={4}>
                  <h4 className="text-danger text-center">Không tìm thấy bài viết nào</h4>
                </td>
              </tr>
            )}
          </tbody>
        </Table>
      )}
      {postsList && postsList.length > 0 && (
        <Pager postQuery={{ ...postQuery }} metadata={metadata} />
      )}
      <VerifyModal
        show={showDeleteModal}
        modalTitle="Xoá bài viết"
        modalBody={`Bạn có chắc chắn muốn xoá bài viết " ${postDelete.title} "?`}
        handleClose={handleCloseDeleteModal}
        onVerify={(e) => deletedChanged(postDelete.id)}
      />
    </>
  );
};

export default Posts;
