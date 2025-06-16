# Social Media Platform API Documentation

This API provides endpoints for a social media platform with user authentication, profiles, posts, comments, and likes functionality.

## Base URL
```
https://api.yourplatform.com
```

## Authentication

All API endpoints (except authentication endpoints) require JWT Bearer token authentication.

### Headers
```
Authorization: Bearer <your_jwt_token>
```

## Authentication Endpoints

### Check Authentication Status
```http
GET /api/auth/auth-check
```
**Description:** Verify if the current token is valid.

**Response:** `200 OK`

### Login
```http
POST /api/auth/login
```
**Description:** Authenticate user and receive JWT tokens.

**Request Body:**
```json
{
  "login": "string",
  "password": "string"
}
```

**Response:** `200 OK`

### Register
```http
POST /api/auth/register
```
**Description:** Create a new user account.

**Request Body:**
```json
{
  "login": "string",
  "password": "string"
}
```

**Response:** `200 OK`

### Refresh Token
```http
POST /api/auth/refresh-token
```
**Description:** Refresh JWT access token using refresh token.

**Response:** `200 OK`

### Revoke Refresh Token
```http
POST /api/auth/revoke-refreshtoken
```
**Description:** Invalidate the current refresh token.

**Response:** `200 OK`

## Profile Endpoints

### Get Current User Profile
```http
GET /api/profiles/me
```
**Description:** Get the authenticated user's profile information.

**Response:** `200 OK`

### Get User Profile by Username
```http
GET /api/profiles/{username}
```
**Description:** Get profile information for a specific user.

**Parameters:**
- `username` (path, required): Username of the user

**Response:** `200 OK`

### Create/Update Profile
```http
POST /api/profiles
```
**Description:** Create or update user profile.

**Request Body (multipart/form-data):**
- `Username` (required): User's username
- `Nickname` (optional): Display name
- `Description` (optional): Profile description/bio
- `Avatar` (optional): Profile picture file

**Response:** `200 OK`

## Post Endpoints

### Get All Posts
```http
GET /api/posts
```
**Description:** Retrieve paginated list of posts.

**Query Parameters:**
- `page` (optional, default: 1): Page number for pagination

**Response:** `200 OK`

### Get Post by ID
```http
GET /api/posts/{id}
```
**Description:** Get a specific post by its ID.

**Parameters:**
- `id` (path, required): Post UUID

**Response:** `200 OK`

### Get Posts by Username
```http
GET /api/posts/by-username/{username}
```
**Description:** Get all posts created by a specific user.

**Parameters:**
- `username` (path, required): Username of the user
- `page` (query, optional, default: 1): Page number for pagination

**Response:** `200 OK`

### Create Post
```http
POST /api/posts
```
**Description:** Create a new post.

**Request Body (multipart/form-data):**
- `Content` (required): Post content/text
- `Medias` (optional): Array of media files (images/videos)

**Response:** `200 OK`

### Update Post
```http
PUT /api/posts/{id}
```
**Description:** Update an existing post.

**Parameters:**
- `id` (path, required): Post UUID

**Request Body (multipart/form-data):**
- `Content` (optional): Updated post content
- `OldMedias` (optional): Array of existing media URLs to keep
- `NewMedias` (optional): Array of new media files to add

**Response:** `200 OK`

### Delete Post
```http
DELETE /api/posts/{id}
```
**Description:** Delete a post.

**Parameters:**
- `id` (path, required): Post UUID

**Response:** `200 OK`

## Comment Endpoints

### Get Comments
```http
GET /api/comments
```
**Description:** Get comments, optionally filtered by post.

**Query Parameters:**
- `postId` (optional): UUID of the post to get comments for
- `page` (optional, default: 1): Page number for pagination

**Response:** `200 OK`

### Get Comment by ID
```http
GET /api/comments/{id}
```
**Description:** Get a specific comment by its ID.

**Parameters:**
- `id` (path, required): Comment UUID

**Response:** `200 OK`

### Get Comments by Username
```http
GET /api/comments/by-username/{username}
```
**Description:** Get all comments created by a specific user.

**Parameters:**
- `username` (path, required): Username of the user
- `page` (query, optional, default: 1): Page number for pagination

**Response:** `200 OK`

### Create Comment
```http
POST /api/comments
```
**Description:** Create a new comment on a post.

**Request Body (multipart/form-data):**
- `PostId` (required): UUID of the post to comment on
- `Content` (optional): Comment text content
- `Medias` (optional): Array of media files

**Response:** `200 OK`

### Update Comment
```http
PUT /api/comments/{id}
```
**Description:** Update an existing comment.

**Parameters:**
- `id` (path, required): Comment UUID

**Request Body (multipart/form-data):**
- `Content` (optional): Updated comment content
- `OldMedias` (optional): Array of existing media URLs to keep
- `NewMedias` (optional): Array of new media files to add

**Response:** `200 OK`

### Delete Comment
```http
DELETE /api/comments/{id}
```
**Description:** Delete a comment.

**Parameters:**
- `id` (path, required): Comment UUID

**Response:** `200 OK`

## Like Endpoints

### Like/Unlike Post
```http
POST /api/likes/posts/{postId}
```
**Description:** Like a post.

**Parameters:**
- `postId` (path, required): Post UUID

**Response:** `200 OK`

```http
DELETE /api/likes/posts/{postId}
```
**Description:** Unlike a post.

**Parameters:**
- `postId` (path, required): Post UUID

**Response:** `200 OK`

### Like/Unlike Comment
```http
POST /api/likes/comments/{commentId}
```
**Description:** Like a comment.

**Parameters:**
- `commentId` (path, required): Comment UUID

**Response:** `200 OK`

```http
DELETE /api/likes/comments/{commentId}
```
**Description:** Unlike a comment.

**Parameters:**
- `commentId` (path, required): Comment UUID

**Response:** `200 OK`

## Data Types

### UUID Format
All IDs use UUID version 4 format:
```
xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx
```

### File Uploads
Media files should be uploaded as `multipart/form-data` with binary format. Supported file types depend on server configuration.

## Error Responses

The API returns appropriate HTTP status codes:
- `200 OK`: Request successful
- `400 Bad Request`: Invalid request data
- `401 Unauthorized`: Authentication required or invalid token
- `403 Forbidden`: Access denied
- `404 Not Found`: Resource not found
- `500 Internal Server Error`: Server error

## Rate Limiting

Please refer to the API response headers for rate limiting information:
- `X-RateLimit-Limit`: Maximum requests allowed
- `X-RateLimit-Remaining`: Remaining requests in current window
- `X-RateLimit-Reset`: Time when rate limit resets

## Examples

### Creating a Post with Media
```bash
curl -X POST "https://api.yourplatform.com/api/posts" \
  -H "Authorization: Bearer your_jwt_token" \
  -F "Content=Check out this amazing sunset!" \
  -F "Medias=@sunset.jpg"
```

### Getting User Posts
```bash
curl -X GET "https://api.yourplatform.com/api/posts/by-username/johndoe?page=1" \
  -H "Authorization: Bearer your_jwt_token"
```

### Liking a Post
```bash
curl -X POST "https://api.yourplatform.com/api/likes/posts/123e4567-e89b-12d3-a456-426614174000" \
  -H "Authorization: Bearer your_jwt_token"
```
