export const AxiosConstants = {
    AXIOS_BASEURL: "http://localhost:5000",
    AXIOS_TIMEOUT: 10000,
    AXIOS_HEADER: {'Content-Type': 'application/json'},
}

export const URLConstants = {
    LOGIN_ENDPOINT: "/login",
    BOOK_GETALL: "/api/book",
    BOOK_GETID: "/api/book/:id",
    BOOK_SEARCH: "/api/book/search",
    BOOK_ADD: "/api/book",
    BOOK_UPDATE: "/api/book",
    BOOK_DELETE: "/api/book"
}

export const LocalStorageConstants = {
    USER_ITEM: "user",
    USER_CREDENTIAL: "user-credential"
}