export const AxiosConstants = {
    AXIOS_BASEURL: "http://localhost:5000",
    AXIOS_TIMEOUT: 10000,
    AXIOS_HEADER: {'Content-Type': 'application/json'},
}

export const URLConstants = {
    LOGIN_ENDPOINT: "/login",
    ACCOUNT_INFO_ENDPOINT: "/api/accounts/info",
    BOOK: {
        GETALL: "/api/books",
        GETID: "/api/books",
        ADD: "/api/books",
        UPDATE: "/api/books",
        DELETE: "/api/books",
    },
    CATEGORY: {
        GETALL: "/api/categories",
        GETID: "/api/categories",
        ADD: "/api/categories",
        UPDATE: "/api/categories",
        DELETE: "/api/categories"
    },
    BORROWING_REQUEST: {
        GETALL_USER: "/api/borrowing/requests/user",
        GETALL_ADMIN: "/api/borrowing/requests",
        GET_CURRENT_MONTH: "/api/borrowing/requests/user/current-month",
        ADD: "/api/borrowing/requests/user",
        UPDATE_STATUS: "/api/borrowing/requests/status"
    }
}

export const LocalStorageConstants = {
    USER_ITEM: "user",
    USER_CREDENTIAL: "user-credential"
}