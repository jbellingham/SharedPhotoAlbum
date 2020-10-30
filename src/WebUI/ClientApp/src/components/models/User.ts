export class User {
    firstName: string | undefined
    lastName: string | undefined
    profilePictureUrl: string | undefined

    constructor(firstName: string | undefined, lastName: string | undefined, profilePictureUrl: string | undefined) {
        this.firstName = firstName
        this.lastName = lastName
        this.profilePictureUrl = profilePictureUrl
    }
}
