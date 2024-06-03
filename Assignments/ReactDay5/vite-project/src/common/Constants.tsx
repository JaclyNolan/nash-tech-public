export const AxiosConstants = {
    AXIOS_BASEURL: "http://localhost:5000",
    AXIOS_TIMEOUT: 10000,
    AXIOS_HEADER: {'Content-Type': 'application/json'},
}

export const URLConstants = {
    LOGIN_ENDPOINT: "/login",
    BOOK: {
        GETALL: "/api/books",
        GETID: "/api/books",
        ADD: "/api/books",
        UPDATE: "/api/books",
        DELETE: "/api/books"
    },
    CATEGORY: {
        GETALL: "/api/categories",
        GETID: "/api/categories",
        ADD: "/api/categories",
        UPDATE: "/api/categories",
        DELETE: "/api/categories"
    }
}

export const LocalStorageConstants = {
    USER_ITEM: "user",
    USER_CREDENTIAL: "user-credential"
}