import React from 'react'
import TagList from './TagList'
import Card from 'react-bootstrap/Card'
import {Link} from 'react-router-dom'
import {isEmptyOrSpaces} from '../Utils/Utils'

export default function PostItem({postItem}) {
  let imageUrl = isEmptyOrSpaces(postItem.imageUrl)
  ?  '../img/image_1.jpg'
  // process.env.PUBLIC_URL +
  : `${postItem.imageUrl}`;

  let postedDate = new Date(postItem.postedDate);

  console.log(postItem);

  return (
  <article className='blog-entry mb-4'>
    <Card>
      <div className='row g-0'>
        <div className='col-md-4'>
          {/* <Card.Img variant='top' src={imageUrl} alt={postItem.title}/> */}
          <Card.Img variant='top' src='../img/image_1.jpg' alt={postItem.title}/>
        </div>
        <div className='col-md-8'>
          <Card.Body>
            <Card.Title>{postItem.title}</Card.Title>
            <Card.Text>
              <small className='text-muted'>Tác giả: </small>
              {/* <Link to={`/blog/post/author/${postItem.author.urlSlug}`}className='text-primary m-1'>{postItem.author.fullName}</Link> */}
              <small className='text-muted'>Chủ đề: </small>
              {/* <Link to={`/blog/post/author/${postItem.category.urlSlug}`}className='text-primary m-1'>{postItem.category.name}</Link> */}
            </Card.Text>
            {/* postItem.author.fullName */}
            <Card.Text>
              {postItem.shortDescription}
            </Card.Text>

            <div className='tag-list'>
              <TagList tagList ={postItem.tags}/>
            </div>

            <div className='text-end'>
              <Link to={`/blog/post?year`}
              // =${postedDate.getFullYear()}&month=${postedDate.getMonth()}&day=${postedDate.getDay()}&slug=${postItem.urlSlug}
              className='btn btn-primary'
              title={postItem.title}>
                Xem chi tiết
              </Link>
            </div>

          </Card.Body>
        </div>
      </div>
    </Card>
  </article>
  )
}
