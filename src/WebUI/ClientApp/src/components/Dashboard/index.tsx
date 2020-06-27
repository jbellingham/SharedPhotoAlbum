import React from 'react'
import { observer } from 'mobx-react'
import { Row, Col } from 'react-bootstrap'
import Post from '../Post'
import NewPost from './NewPost'
import { useStore } from '../../stores/StoreContext'

const Dashboard = observer(() => {
    const { postStore } = useStore()
    const posts = postStore.posts

    return (
        <div className="dashboard-container">
            <Row>
                <Col md={{ span: 4, offset: 4 }}>
                    <NewPost />
                </Col>
            </Row>
            {posts &&
                posts.map((post) => {
                    return (
                        <Row key={post.id}>
                            <Col md={{ span: 4, offset: 4 }}>
                                <Post {...post} />
                            </Col>
                        </Row>
                    )
                })}
        </div>
    )
})

export default Dashboard
