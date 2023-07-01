import { User } from "./user";

export class UserParams {
    gender: string;
    minAge = 12;
    maxAge = 99;
    pageNumber = 1;
    pageSize = 6;
    orderBy = 'LastActive';

    constructor(user: User) {
        this.gender = user.gender === 'female' ? 'male' : 'female'
    }
}