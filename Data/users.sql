DROP TABLE IF EXISTS users;
CREATE TABLE "users" (
    "id" SERIAL8 PRIMARY KEY,
    "name" VARCHAR(20) NOT NULL,
    "about" VARCHAR(255),
    "phone" INT8 UNIQUE NOT NULL,
    "email" VARCHAR(255),
    "profile_url" VARCHAR(255),
    "created_at" TIMESTAMPTZ DEFAULT NOW(),
    "updated_at" TIMESTAMPTZ 
);